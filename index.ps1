$Env:PSModulePath += ";$PSScriptRoot\Modules"
$PSDefaultParameterValues = Import-PowerShellDataFile "$PSScriptRoot\Overrides\Param.psd1" -ErrorAction Stop
[void](. $PSScriptRoot\Overrides\Alias.ps1)
