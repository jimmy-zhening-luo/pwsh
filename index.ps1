$Env:PSModulePath += ";$PSScriptRoot\Modules"

try {
  $PSDefaultParameterValues = Import-PowerShellDataFile "$PSScriptRoot\Overrides\Param.psd1" -ErrorAction Stop
  . $PSScriptRoot\Overrides\Alias.ps1
}
catch {
  throw "Failed to initialize PowerShell profile: $($_.Exception.Message)"
}
