using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class GenericCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Units
  [string] $Case

  GenericCompletionsAttribute(
    [string] $units
  ) {
    $this.Units = $units
    $this.Case = 'Lower'
  }
  GenericCompletionsAttribute(
    [string] $units,
    [string] $case
  ) {
    $this.Units = $units
    $this.Case = $case
  }

  [IArgumentCompleter] Create() {
    return [GenericCompleter]::new(
      $this.Units,
      $this.Case
    )
  }
}

class GenericCompleter : IArgumentCompleter {
  [string] $Units
  [string] $Case

  GenericCompleter(
    [string] $units,
    [string] $case
  ) {
    if (
      -not $case -or $case -notin @(
        'Lower'
        'Upper'
        'Preserve'
      )
    ) {
      throw [ArgumentException]::new('case')
    }

    if (-not $units) {
      throw [ArgumentException]::new('units')
    }

    $unitKeys = (
      $units -split ','
    ).Trim().ToLowerInvariant() -notmatch '^\s*$'

    if (-not $unitKeys) {
      throw [ArgumentException]::new('units')
    }

    $unique = @()
    $unique += $unitKeys |
      Select-Object -Unique

    if (-not $unique -or $unique.Count -ne $unitKeys.Count) {
      throw [ArgumentException]::new('units')
    }

    $this.Units = $units
    $this.Case = $case
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters
  ) {

    $Local:units = (
      $this.Units -split ','
    ).Trim() -notmatch '^\s*$'

    switch ($this.Case) {
      Lower {
        $Local:units = $Local:units.ToLowerInvariant()
        break
      }
      Upper {
        $Local:units = $Local:units.ToUpperInvariant()
      }
    }

    $unitMatches = @()

    if ($Local:units) {
      if ($wordToComplete) {
        $unitMatches += $Local:units |
          ? { $_ -like "$wordToComplete*" }

        if (-not $unitMatches) {
          $unitMatches += $Local:units |
            ? { $_ -like "*$wordToComplete*" }
        }
        elseif ($unitMatches.Count -eq 1) {
          if ($unitMatches[0] -eq $wordToComplete) {
            $exactMatch, $unitMatches = $unitMatches[0], @()
            $unitMatches += (
              $Local:units |
                ? { $_ -like "*$wordToComplete*" }
            ) -ne $exactMatch
            $unitMatches += $exactMatch
          }
          else {
            $unitMatches += $wordToComplete
          }
        }
      }
      else {
        $unitMatches += $Local:units
      }
    }

    $resultList = [List[CompletionResult]]::new()

    foreach ($unit in $unitMatches) {
      $resultList.Add([CompletionResult]::new($unit))
    }

    return $resultList
  }
}

$ExportableTypes = @(
  [GenericCompletionsAttribute]
  [GenericCompleter]
)
$TypeAcceleratorsClass = [PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    throw [System.Management.Automation.ErrorRecord]::new(
      [InvalidOperationException]::new("Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."),
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
