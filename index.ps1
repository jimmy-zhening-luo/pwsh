$PSDefaultParameterValues = Import-PowerShellDataFile -ErrorAction Stop "$PSScriptRoot\defaults.psd1"
$Env:PSModulePath += ";$PSScriptRoot\Modules"

try {
  . $PSScriptRoot\data\index.ps1
  . $PSScriptRoot\alias\index.ps1
  . $PSScriptRoot\profile.ps1
}
catch {
  throw "Failed to initialize PowerShell profile: $($_.Exception.Message)"
}
