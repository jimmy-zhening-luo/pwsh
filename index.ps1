$Env:PSModulePath += ";$PSScriptRoot\Module"

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1

. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Install.ps1
