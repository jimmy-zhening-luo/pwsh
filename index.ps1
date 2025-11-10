$Env:PSModulePath += ";$PSScriptRoot\Modules"

$PSDefaultParameterValues = Import-PowerShellDataFile -Path "$PSScriptRoot\Overrides\Param.psd1" -ErrorAction Stop

[void](. $PSScriptRoot\Overrides\Alias.ps1)
