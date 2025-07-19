$code = "$HOME\code"

$PSDefaultParameterValues = Import-PowerShellDataFile "$PSScriptRoot\defaults.psd1"
$Env:PSModulePath += ";$PSScriptRoot\Modules"

. $PSScriptRoot\profile.ps1

. $PSScriptRoot\data\index.ps1
. $PSScriptRoot\alias\index.ps1
