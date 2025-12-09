$Env:PSModulePath += ";$PSScriptRoot\Modules"

$PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Overrides\Parameter.psd1

. $PSScriptRoot\Overrides\Alias.ps1
