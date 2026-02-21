$Env:PSModulePath += "$PSScriptRoot\Module;"

$Global:InformationPreference = 'Continue'
$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1

. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Install.ps1

if ($null -ne $Env:SSH_CLIENT) {
  if ($HOME -eq $PWD.Path) {
    Set-Location code
  }

  . $PSScriptRoot\Script\Key.ps1
}
