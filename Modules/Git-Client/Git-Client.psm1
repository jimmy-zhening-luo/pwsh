class GitVerb : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Import-PowerShellDataFile (Join-Path $PSScriptRoot "Git-Verb.psd1" -Resolve)).GIT_VERB
  }
}

$ExportableTypes = @(
  [GitVerb]
)
$TypeAcceleratorsClass = [PSObject].Assembly.GetType(
  'System.Management.Automation.TypeAccelerators'
)
$ExistingTypeAccelerators = $TypeAcceleratorsClass::Get
foreach ($Type in $ExportableTypes) {
  if ($Type.FullName -in $ExistingTypeAccelerators.Keys) {
    throw [System.Management.Automation.ErrorRecord]::new(
      [System.InvalidOperationException]::new("Unable to register type accelerator '$($Type.FullName)' - Accelerator already exists."),
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
