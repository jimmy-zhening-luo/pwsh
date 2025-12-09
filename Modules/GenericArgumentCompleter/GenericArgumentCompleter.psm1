using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class GenericCompleterBase {
  static [List[string]] FindCompletion(
    [List[string]] $values,
    [string] $wordToComplete
  ) {
    $completions = [List[string]]::new()

    $currentArgumentText = $wordToComplete ? $wordToComplete -match "^'(?<CurrentText>.*)'$" ? $Matches.CurrentText -replace "''", "'" : $wordToComplete : ''

    if ($currentArgumentText) {
      $tailCompletions = $values |
        Where-Object { $PSItem -like "$currentArgumentText*" }

      if ($tailCompletions) {
        $completions.AddRange([List[string]]$tailCompletions)
      }

      if ($completions.Count -eq 0 -or $completions.Count -eq 1 -and $completions[0] -eq $currentArgumentText) {
        $surroundingCompletions = $values |
          Where-Object { $PSItem -like "*$currentArgumentText*" } |
          Where-Object { $PSItem -ne $currentArgumentText }

        if ($surroundingCompletions) {
          $completions.AddRange([List[string]]$surroundingCompletions)
        }
      }
    }
    else {
      $completions.AddRange($values)
    }

    return $completions
  }

  static [List[CompletionResult]] CreateCompletion(
    [List[string]] $completions
  ) {
    $completionResults = [List[CompletionResult]]::new()

    foreach ($completion in $completions) {
      $escapedCompletion = [CodeGeneration]::EscapeSingleQuotedStringContent($completion)
      $quotedEscapedCompletion = $escapedCompletion -match '\s' ? "'" + $escapedCompletion + "'" : $escapedCompletion

      $completionResults.Add(
        [CompletionResult]::new(
          $quotedEscapedCompletion
        )
      )
    }

    return $completionResults
  }
}

class GenericCompleter : GenericCompleterBase, IArgumentCompleter {
  [List[string]] $Units
  [bool] $Sort
  [string] $Case

  GenericCompleter(
    [List[string]] $units,
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
      throw [ArgumentException]::new('case')
    }

    if ($units.Count -eq 0) {
      throw [ArgumentException]::new('units')
    }

    $unique = @()
    $unique += $units |
      Select-Object -Unique -CaseInsensitive

    if (-not $unique -or $unique.Count -ne $units.Count) {
      throw [ArgumentException]::new('units')
    }

    $this.Units = $units
    $this.Sort = $sort
    $this.Case = $case
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters
  ) {

    $Local:units = [List[string]]::new()

    switch ($this.Case) {
      Lower {
        $Local:units.AddRange([List[string]]$this.Units.ToLowerInvariant())
        break
      }
      Upper {
        $Local:units.AddRange([List[string]]$this.Units.ToUpperInvariant())
      }
    }

    if ($this.Sort) {
      $Local:units.Sort()
    }

    return [GenericCompleter]::CreateCompletion(
      [GenericCompleter]::FindCompletion(
        $Local:units,
        "$wordToComplete"
      )
    )
  }
}

class GenericCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
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

  [IArgumentCompleter] Create() {
    $unitList = [List[string]]::new()
    $parsedUnits = ($this.Units -split ',').Trim() |
      Where-Object { $PSItem }

    if ($parsedUnits) {
      $unitList.AddRange([List[string]]$parsedUnits)
    }

    return [GenericCompleter]::new(
      $unitList,
      $this.Sort,
      $this.Case
    )
  }
}

$ExportableTypes = @(
  [GenericCompleterBase]
  [GenericCompleter]
  [GenericCompletionsAttribute]
)
$TypeAcceleratorsClass = [PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    $Message = @(
      "Unable to register type accelerator '$($Type.FullName)'"
      'Accelerator already exists.'
    ) -join ' - '
    throw [System.Management.Automation.ErrorRecord]::new(
      [System.InvalidOperationException]::new($Message),
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
