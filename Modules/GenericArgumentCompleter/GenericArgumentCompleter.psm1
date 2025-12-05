using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language

class GenericCompletionsAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  [string] $Units

  GenericCompletionsAttribute(
    [string] $units
  ) {
    $this.Units = $units
  }

  [IArgumentCompleter] Create() {
    return [GenericCompleter]::new(
      $this.Units
    )
  }
}

class GenericCompleter : IArgumentCompleter {
  [string] $Units

  GenericCompleter(
    [string] $units
  ) {
    if (-not $units) {
      throw [ArgumentException]::new('units')
    }

    $this.Units = $units
  }

  [IEnumerable[CompletionResult]] CompleteArgument(
    [string] $CommandName,
    [string] $parameterName,
    [string] $wordToComplete,
    [CommandAst] $commandAst,
    [IDictionary] $fakeBoundParameters
  ) {

    $Local:units = ($this.Units -split ',').Trim().ToLowerInvariant() |
      ? { -not [string]::IsNullOrWhiteSpace($_) } |
      Select-Object -Unique
    $unitMatches = @()
    $resultList = [List[CompletionResult]]::new()

    if ($wordToComplete) {
      $unitMatches = $Local:units |
        ? { $_ -like "$wordToComplete*" }
    }

    if (-not $unitMatches) {
      $unitMatches = $Local:units
    }

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
