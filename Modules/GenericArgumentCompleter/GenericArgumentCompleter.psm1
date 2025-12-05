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
