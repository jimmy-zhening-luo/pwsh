$PSDefaultParameterValues = Import-PowerShellDataFile -ErrorAction Stop "$PSScriptRoot\defaults.psd1"
$Env:PSModulePath += ";$PSScriptRoot\Modules"
$code = "$HOME\code"
$PROFILE_REPO = "$code\pwsh"

try {
  . $PSScriptRoot\data\index.ps1
  . $PSScriptRoot\overrides.ps1
}
catch {
  throw "Failed to initialize PowerShell profile: $($_.Exception.Message)"
}
