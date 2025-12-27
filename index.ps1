if (-not $Env:PSModulePath.EndsWith(";$PSScriptRoot\Module;")) {
  $Env:PSModulePath += ";$PSScriptRoot\Module;"
}

if (-not -not $Env:SSH_CLIENT) {
  $PSSessionOption.NoCompression = $True
}

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1

$Script:REPO_ROOT = Split-Path $PSScriptRoot

. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Key.ps1
. $PSScriptRoot\Script\Native.ps1
. $PSScriptRoot\Script\Install.ps1
