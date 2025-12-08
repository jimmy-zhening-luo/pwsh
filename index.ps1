$Env:PSModulePath += ";$PSScriptRoot\Modules"

$PSDefaultParameterValues = Microsoft.PowerShell.Utility\Import-PowerShellDataFile -Path $PSScriptRoot\Overrides\Parameter.psd1

[void](. $PSScriptRoot\Overrides\Alias.ps1)
