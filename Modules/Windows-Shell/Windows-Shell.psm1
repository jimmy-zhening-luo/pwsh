class SiblingItem : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Split-Path $PWD | Get-ChildItem).BaseName
  }
}

class SiblingFolder : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Split-Path $PWD | Get-ChildItem -Directory).BaseName
  }
}

class RelativeItem : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Split-Path (Split-Path $PWD) | Get-ChildItem).BaseName
  }
}

class RelativeFolder : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Split-Path (Split-Path $PWD) | Get-ChildItem -Directory).BaseName
  }
}

class HomeItem : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Get-ChildItem '~').BaseName
  }
}

class HomeFolder : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] (Get-ChildItem -Directory '~').BaseName
  }
}

class DriveItem : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] ($PWD.Drive.Root | Get-ChildItem).BaseName
  }
}

class DriveFolder : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] ($PWD.Drive.Root | Get-ChildItem -Directory).BaseName
  }
}

class DriveDItem : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] ((Test-Path 'D:\') ? ('D:\' | Get-ChildItem).BaseName : @())
  }
}

class DriveDFolder : System.Management.Automation.IValidateSetValuesGenerator {
  [System.String[]] GetValidValues() {
    return [System.String[]] ((Test-Path 'D:\') ? ('D:\' | Get-ChildItem -Directory).BaseName : @())
  }
}

$ExportableTypes = @(
  [SiblingItem]
  [SiblingFolder]
  [RelativeItem]
  [RelativeFolder]
  [HomeItem]
  [HomeFolder]
  [DriveItem]
  [DriveFolder]
  [DriveDItem]
  [DriveDFolder]
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
