$Env:PSModulePath += ";$PSScriptRoot\Modules"

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Overrides\Parameter.psd1

. $PSScriptRoot\Overrides\Alias.ps1
