. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Install.ps1

if (-not -not $Env:SSH_CLIENT) {
  . $PSScriptRoot\Script\Key.ps1
}

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1

$Script:REPO_ROOT = Split-Path $PSScriptRoot

if (-not $Env:PSModulePath.EndsWith(";$PSScriptRoot\Module;")) {
  $Env:PSModulePath += ";$PSScriptRoot\Module;"
}
