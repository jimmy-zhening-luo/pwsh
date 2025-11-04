class SiblingItem : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (Split-Path $PWD.Path | Get-ChildItem).BaseName
  }
}

class SiblingDirectory : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (Split-Path $PWD.Path | Get-ChildItem -Directory).BaseName
  }
}

class RelativeItem : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (Split-Path (Split-Path $PWD.Path) | Get-ChildItem).BaseName
  }
}

class RelativeDirectory : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (Split-Path (Split-Path $PWD.Path) | Get-ChildItem -Directory).BaseName
  }
}

class HomeItem : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (Get-ChildItem '~').BaseName
  }
}

class HomeDirectory : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] (Get-ChildItem -Directory '~').BaseName
  }
}

class DriveItem : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] ($PWD.Drive.Root | Get-ChildItem).BaseName
  }
}

class DriveDirectory : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] ($PWD.Drive.Root | Get-ChildItem -Directory).BaseName
  }
}

class DriveDItem : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] ((Test-Path 'D:\') ? ('D:\' | Get-ChildItem).BaseName : @())
  }
}

class DriveDDirectory : System.Management.Automation.IValidateSetValuesGenerator {
  [string[]] GetValidValues() {
    return [string[]] ((Test-Path 'D:\') ? ('D:\' | Get-ChildItem -Directory).BaseName : @())
  }
}

$ExportableTypes = @(
  [SiblingItem]
  [SiblingDirectory]
  [RelativeItem]
  [RelativeDirectory]
  [HomeItem]
  [HomeDirectory]
  [DriveItem]
  [DriveDirectory]
  [DriveDItem]
  [DriveDDirectory]
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
