$Env:PSModulePath += ";$PSScriptRoot\Modules"

try {
  $PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Overrides\Parameter.psd1 -ErrorAction Stop

  [void](. $PSScriptRoot\Overrides\Alias.ps1)
}
catch {
  throw "Failed to initialize PowerShell profile.`n$($_.Exception.Message)"
}
