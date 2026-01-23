if (-not $Env:PSModulePath.StartsWith(";$PSScriptRoot\Module")) {
  Write-Warning -Message "Appended module path to current module path: $Env:PSModulePath"
  $Env:PSModulePath += ";$PSScriptRoot\Module"
}

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1

$Script:REPO_ROOT = Split-Path $PSScriptRoot

. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Install.ps1

if ($null -ne $Env:SSH_CLIENT) {
  if ($HOME -eq $PWD) {
    Set-Location code
  }

  . $PSScriptRoot\Script\Key.ps1
}
