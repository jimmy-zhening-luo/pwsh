using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language
using namespace CompleterBase

class CompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {

  [scriptblock] $Units

  [CompletionCase] $Case

  [bool] $Sort

  [bool] $Surrounding

  CompletionsAttribute(
    [scriptblock] $units
  ) {
    $this.Units = $units
    $this.Case = [CompletionCase]::Lower
    $this.Sort = $False
    $this.Surrounding = $True
  }
  CompletionsAttribute(
    [scriptblock] $units,
    [CompletionCase] $case
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $False
    $this.Surrounding = $True
  }
  CompletionsAttribute(
    [scriptblock] $units,
    [CompletionCase] $case,
    [bool] $sort
  ) {
    $this.Units = $units
    $this.Case = $case
    $this.Sort = $sort
    $this.Surrounding = $True
  }
  CompletionsAttribute(
    [scriptblock] $units,
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

    return [Completer]::new(
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
