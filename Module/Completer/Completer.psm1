using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class Completer : CompleterBase.CompleterBase, IArgumentCompleter {

  [List[string]] $Units

  [CompleterBase.CompletionCase] $Case

  [bool] $Sort

  [bool] $Surrounding

  Completer(

    [List[string]] $units,

    [CompleterBase.CompletionCase] $case,

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
    return [Completer]::CreateCompletionResult(
      [Completer]::FindCompletion(
        $this.Units,
        $wordToComplete,
        $this.Case,
        $this.Sort,
        $this.Surrounding
      )
    )
  }
}

class CompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {

  [scriptblock] $Units

  [CompleterBase.CompletionCase] $Case

  [bool] $Sort

  [bool] $Surrounding

  CompletionsAttribute(
    [scriptblock] $units
  ) {
    $this.Units = $units
    $this.Case = [CompleterBase.CompletionCase]::Lower
    $this.Sort = $False
    $this.Surrounding = $True
  }
  CompletionsAttribute(
    [scriptblock] $units,
    [CompleterBase.CompletionCase] $case
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $False
    $this.Surrounding = $True
  }
  CompletionsAttribute(
    [scriptblock] $units,
    [CompleterBase.CompletionCase] $case,
    [bool] $sort
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $sort
    $this.Surrounding = $True
  }
  CompletionsAttribute(
    [scriptblock] $units,
    [CompleterBase.CompletionCase] $case,
    [bool] $sort,
    [bool] $surrounding
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $sort
    $this.Surrounding = $surrounding
  }

  [IArgumentCompleter] Create() {
    $private:unitList = [List[string]]::new()

    [string[]]$private:invokedUnits = Invoke-Command -ScriptBlock $this.Units

    if ($invokedUnits) {
      [string[]]$private:cleanedUnits = $invokedUnits.Trim() |
        Where-Object {
          -not [string]::IsNullOrEmpty($PSItem)
        }

      if ($cleanedUnits) {
        $unitList.AddRange(
          [List[string]]$cleanedUnits
        )
      }
    }

    return [CompleterBase.CompleterBase]::new(
      $unitList,
      $this.Case,
      $this.Sort,
      $this.Surrounding
    )
  }
}

$ExportableTypes = @(
  [CompletionsAttribute]
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
