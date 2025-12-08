using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class GenericCompletionsAttribute : System.Management.Automation.ArgumentCompleterAttribute, System.Management.Automation.IArgumentCompleterFactory {
  [string] $Units
  [bool] $Sort
  [string] $Case

  GenericCompletionsAttribute(
    [string] $units
  ) {
    $this.Units = $units
    $this.Sort = $False
    $this.Case = 'Lower'
  }
  GenericCompletionsAttribute(
    [string] $units,
    [bool] $sort
  ) {
    $this.Units = $units
    $this.Sort = $sort
    $this.Case = 'Lower'
  }
  GenericCompletionsAttribute(
    [string] $units,
    [bool] $sort,
    [string] $case
  ) {
    $this.Units = $units
    $this.Sort = $sort
    $this.Case = $case
  }

  [System.Management.Automation.IArgumentCompleter] Create() {
    return [GenericCompleter]::new(
      $this.Units,
      $this.Sort,
      $this.Case
    )
  }
}

class GenericCompleter : System.Management.Automation.IArgumentCompleter {
  [string] $Units
  [bool] $Sort
  [string] $Case

  GenericCompleter(
    [string] $units,
    [bool] $sort,
    [string] $case
  ) {
    if (
      -not $case -or $case -notin @(
        'Lower'
        'Upper'
        'Preserve'
      )
    ) {
      throw [System.ArgumentException]::new('case')
    }

    if (-not $units) {
      throw [System.ArgumentException]::new('units')
    }

    $unitKeys = (
      $units -split ','
    ).Trim() | Where-Object { $PSItem }

    if (-not $unitKeys) {
      throw [System.ArgumentException]::new('units')
    }

    $unique = @()
    $unique += $unitKeys |
      Microsoft.PowerShell.Utility\Select-Object -Unique -CaseInsensitive

    if (-not $unique -or $unique.Count -ne $unitKeys.Count) {
      throw [System.ArgumentException]::new('units')
    }

    if ($unique -match "^'.*'$") {
      throw [System.ArgumentException]::new('units')
    }

    $this.Units = $units
    $this.Sort = $sort
    $this.Case = $case
  }

  [System.Collections.Generic.IEnumerable[System.Management.Automation.CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [System.Management.Automation.Language.CommandAst] $commandAst,
    [System.Collections.IDictionary] $fakeBoundParameters
  ) {

    $Local:units = (
      $this.Units -split ','
    ).Trim() | Where-Object { $PSItem }

    switch ($this.Case) {
      Lower {
        $Local:units = $Local:units.ToLowerInvariant()
        break
      }
      Upper {
        $Local:units = $Local:units.ToUpperInvariant()
      }
    }

    if ($this.Sort) {
      $Local:units = $Local:units | Microsoft.PowerShell.Utility\Sort-Object
    }

    $unitMatches = @()

    $currentText = $wordToComplete ? $wordToComplete -match "^'(?<CurrentText>.*)'$" ? $Matches.CurrentText -replace "''", "'" : $wordToComplete : ''

    if ($currentText) {
      $unitMatches += $Local:units |
        Where-Object { $PSItem -like "$currentText*" }

      if (-not $unitMatches) {
        $unitMatches += $Local:units |
          Where-Object { $PSItem -like "*$currentText*" }
      }
      elseif ($unitMatches.Count -eq 1) {
        if ($unitMatches[0] -eq $currentText) {
          $exactMatch, $unitMatches = $unitMatches[0], @()
          $unitMatches += (
            $Local:units |
              Where-Object { $PSItem -like "*$currentText*" } |
              Where-Object { $PSItem -ne $exactMatch }
          )
          $unitMatches += $exactMatch

          if ($unitMatches.Count -eq 1) {
            $exactMatch, $unitMatches = $unitMatches[0], @()

            if ($currentText -cne $exactMatch) {
              $unitMatches += $exactMatch
            }
          }
        }
        else {
          $unitMatches += $currentText
        }
      }
    }
    else {
      $unitMatches += $Local:units
    }

    $resultList = [System.Collections.Generic.List[System.Management.Automation.CompletionResult]]::new()

    foreach ($item in $unitMatches) {
      $string = [System.Management.Automation.Language.CodeGeneration]::EscapeSingleQuotedStringContent($item)
      $completion = $string -match '\s' ? "'" + $string + "'" : $string

      $resultList.Add(
        [System.Management.Automation.CompletionResult]::new(
          $completion
        )
      )
    }

    return $resultList
  }
}

$ExportableTypes = @(
  [GenericCompletionsAttribute]
  [GenericCompleter]
)
$TypeAcceleratorsClass = [System.Management.Automation.PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    throw [System.Management.Automation.ErrorRecord]::new(
      [System.Management.Automation.InvalidOperationException]::new("Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."),
      'TypeAcceleratorAlreadyExists',
      [System.Management.Automation.ErrorCategory]::InvalidOperation,
      $Type.FullName
    )
  }
}
foreach ($Type in $ExportableTypes) {
  $TypeAcceleratorsClass::Add($Type.FullName, $Type)
}
$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  foreach ($Type in $ExportableTypes) {
    $TypeAcceleratorsClass::Remove($Type.FullName)
  }
}.GetNewClosure()
