$Env:PSModulePath += ";$PSScriptRoot\Module"

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1

$Script:REPO_ROOT = Split-Path $PSScriptRoot

. $PSScriptRoot\Script\Install.ps1
. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Native.ps1

Write-Output $env:SSH_CLIENT
