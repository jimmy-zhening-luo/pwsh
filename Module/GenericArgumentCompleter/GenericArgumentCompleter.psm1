using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

enum CompletionCase {
  Preserve
  Lower
  Upper
}

class GenericCompleterBase {
  static [string] Unescape(

    [string] $escapedValue

  ) {
    if ($escapedValue.Length -gt 1 -and $escapedValue.StartsWith("'") -and $escapedValue.EndsWith("'")) {
      return $escapedValue.Substring(
        1,
        $escapedValue.Length - 2
      ).Replace(
        "''",
        "'"
      )
    }
    else {
      return $escapedValue
    }
  }

  static [string] Escape(

    [string] $value

  ) {
    if ($value.Contains(' ')) {
      return "'" + [CodeGeneration]::EscapeSingleQuotedStringContent($value) + "'"
    }
    else {
      return $value
    }
  }

  static [List[string]] FindCompletion(

    [List[string]] $values,

    [string] $wordToComplete

  ) {
    return [GenericCompleterBase]::FindCompletion(
      $values,
      $wordToComplete,
      [CompletionCase]::Preserve,
      $False,
      $False
    )
  }

  static [List[string]] FindCompletion(

    [List[string]] $values,

    [string] $wordToComplete,

    [CompletionCase] $case

  ) {
    return [GenericCompleterBase]::FindCompletion(
      $values,
      $wordToComplete,
      $case,
      $False,
      $False
    )
  }

  static [List[string]] FindCompletion(

    [List[string]] $values,

    [string] $wordToComplete,

    [CompletionCase] $case,

    [bool] $sort

  ) {
    return [GenericCompleterBase]::FindCompletion(
      $values,
      $wordToComplete,
      $case,
      $sort,
      $False
    )
  }

  static [List[string]] FindCompletion(

    [List[string]] $values,

    [string] $wordToComplete,

    [CompletionCase] $case,

    [bool] $sort,

    [bool] $surrounding

  ) {
    $private:completions = [List[string]]::new()

    $private:normalizedValues = [List[string]]::new()

    switch ($case) {
      Preserve {
        $normalizedValues.AddRange($values)
      }
      Lower {
        $normalizedValues.AddRange([List[string]]$values.ToLowerInvariant())
      }
      Upper {
        $normalizedValues.AddRange([List[string]]$values.ToUpperInvariant())
      }
    }

    if ($sort) {
      $normalizedValues.Sort()
    }

    [string]$private:currentValue = [GenericCompleterBase]::Unescape($wordToComplete)

    if ($currentValue) {
      [string[]]$private:tailCompletions = $normalizedValues -like "$currentValue*"

      if ($tailCompletions) {
        $completions.AddRange([List[string]]$tailCompletions)
      }

      if (
        $surrounding -and (
          $completions.Count -eq 0 -or $completions.Count -eq 1 -and $currentValue -in $completions
        )
      ) {
        [string[]]$private:surroundingCompletions = $normalizedValues -like "*$currentValue*" -ne $currentValue

        if ($surroundingCompletions) {
          $completions.AddRange([List[string]]$surroundingCompletions)
        }
      }
    }
    else {
      $completions.AddRange($normalizedValues)
    }

    return $completions
  }

  static [List[CompletionResult]] CreateCompletionResult(

    [List[string]] $completions

  ) {
    $private:completionResults = [List[CompletionResult]]::new()

    foreach ($private:completion in $completions) {
      $completionResults.Add(
        [CompletionResult]::new(
          [GenericCompleterBase]::Escape(
            $completion
          )
        )
      )
    }

    return $completionResults
  }
}

class GenericCompleter : GenericCompleterBase, IArgumentCompleter {

  [List[string]] $Units

  [CompletionCase] $Case

  [bool] $Sort

  [bool] $Surrounding

  GenericCompleter(

    [List[string]] $units,

    [CompletionCase] $case,

    [bool] $sort,

    [bool] $surrounding

  ) {
    if ($units.Count -eq 0) {
      throw [ArgumentException]::new('units')
    }

    [string[]]$private:unique = $units |
      Select-Object -Unique -CaseInsensitive

    if (-not $unique -or $unique.Count -ne $units.Count) {
      throw [ArgumentException]::new('units')
    }

    $this.Units = $units
    $this.Case = $case
    $this.Sort = $sort
    $this.Surrounding = $surrounding
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters
  ) {
    return [GenericCompleter]::CreateCompletionResult(
      [GenericCompleter]::FindCompletion(
        $this.Units,
        $wordToComplete,
        $this.Case,
        $this.Sort,
        $this.Surrounding
      )
    )
  }
}

class GenericCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {

  [string] $Units

  [CompletionCase] $Case

  [bool] $Sort

  [bool] $Surrounding

  GenericCompletionsAttribute(
    [string] $units
  ) {
    $this.Units = $units
    $this.Case = [CompletionCase]::Lower
    $this.Sort = $False
    $this.Surrounding = $True
  }
  GenericCompletionsAttribute(
    [string] $units,
    [CompletionCase] $case
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $False
    $this.Surrounding = $True
  }
  GenericCompletionsAttribute(
    [string] $units,
    [CompletionCase] $case,
    [bool] $sort
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $sort
    $this.Surrounding = $True
  }
  GenericCompletionsAttribute(
    [string] $units,
    [CompletionCase] $case,
    [bool] $sort,
    [bool] $surrounding
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $sort
    $this.Surrounding = $surrounding
  }

  [IArgumentCompleter] Create() {
    [string[]]$private:parsedUnits = ($this.Units -split ',').Trim() |
      Where-Object {
        -not [string]::IsNullOrEmpty($PSItem)
      }

    $private:unitList = [List[string]]::new(
      [List[string]]$parsedUnits
    )

    return [GenericCompleter]::new(
      $unitList,
      $this.Case,
      $this.Sort,
      $this.Surrounding
    )
  }
}

$ExportableTypes = @(
  [CompletionCase]
  [GenericCompleterBase]
  [GenericCompleter]
  [GenericCompletionsAttribute]
)

$TypeAcceleratorsClass = [PSObject].Assembly.GetType('System.Management.Automation.TypeAccelerators')

$Private:ExistingTypeAccelerators = $TypeAcceleratorsClass::Get

foreach ($Private:Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    [string]$Private:Message = @(
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

foreach ($Private:Type in $ExportableTypes) {
  $TypeAcceleratorsClass::Add($Type.FullName, $Type)
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  foreach ($Private:Type in $ExportableTypes) {
    $TypeAcceleratorsClass::Remove($Type.FullName)
  }
}.GetNewClosure()
