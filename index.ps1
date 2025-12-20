if (-not $Env:PSModulePath.EndsWith(";$PSScriptRoot\Module;")) {
  $Env:PSModulePath += ";$PSScriptRoot\Module;"
}

if (-not $Global:PSDefaultParameterValue) {
  $Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1
}

$Script:REPO_ROOT = Split-Path $PSScriptRoot

. $PSScriptRoot\Script\Install.ps1
. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Native.ps1

echo "Invoked"