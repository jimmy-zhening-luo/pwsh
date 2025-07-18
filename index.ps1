$code = "$HOME\code"
$PROFILE_REPO = "$code\pwsh"
$Env:PSModulePath += ";$PROFILE_REPO\modules"

$PSDefaultParameterValues = Import-PowerShellDataFile "$PSScriptRoot\defaults.psd1"

. $PSScriptRoot\profile.ps1

. $PSScriptRoot\data\index.ps1
. $PSScriptRoot\alias\index.ps1
