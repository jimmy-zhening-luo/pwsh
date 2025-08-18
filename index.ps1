$PSDefaultParameterValues = Import-PowerShellDataFile -ErrorAction Stop "$PSScriptRoot\defaults.psd1"
$Env:PSModulePath += ";$PSScriptRoot\Modules"
$DEV_DRIVE = 'V:'
$CODE = "$DEV_DRIVE\code"
$PROFILE_SRC = "$CODE\pwsh"

try {
  . $PSScriptRoot\data\index.ps1
  . $PSScriptRoot\overrides.ps1
}
catch {
  throw "Failed to initialize PowerShell profile: $($_.Exception.Message)"
}
