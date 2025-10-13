$Env:PSModulePath += ";$PSScriptRoot\Modules"
$DEV_DRIVE = $HOME
$CODE = "$DEV_DRIVE\code"
$PROFILE_SRC = "$CODE\pwsh"

try {
  $PSDefaultParameterValues = Import-PowerShellDataFile "$PSScriptRoot\defaults.psd1" -ErrorAction Stop
  . $PSScriptRoot\overrides.ps1
}
catch {
  throw "Failed to initialize PowerShell profile: $($_.Exception.Message)"
}
