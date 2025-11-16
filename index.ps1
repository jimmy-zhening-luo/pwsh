$Env:PSModulePath += ";$PSScriptRoot\Modules"

try {
  $PSDefaultParameterValues = Import-PowerShellDataFile -Path "$PSScriptRoot\Overrides\Param.psd1" -ErrorAction Stop

  [void](. $PSScriptRoot\Overrides\Alias.ps1)
  [void](. $PSScriptRoot\Overrides\Native.ps1)
}
catch {
  throw "Failed to initialize PowerShell profile.`n$($_.Exception.Message)"
}
