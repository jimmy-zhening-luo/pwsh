function Resolve-NodeProject {
  [OutputType([string])]
  param([string]$Path = ".")

  $PKG = "package.json"
  $PkgPath = (Join-Path $Path $PKG)

  if (Test-Path $PkgPath -PathType Leaf) {
    Resolve-Path $Path |
      Select-Object -ExpandProperty Path
  }
  else { '' }
}

class Repository : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (
      Get-ChildItem -Directory $Script:CODE |
        Select-Object -ExpandProperty BaseName
    )
  }
}

$ExportableTypes = @(
  [Repository]
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
