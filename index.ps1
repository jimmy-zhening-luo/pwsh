$Env:PSModulePath += "$PSScriptRoot\Module;"

$Global:InformationPreference = 'Continue'
$Global:PSDefaultParameterValues = @{
  'Format-Table:Wrap'             = $True
  'Format-Table:HideTableHeaders' = $True
  'Update-Help:Scope'             = 'AllUsers'
  'Get-AppxPackage:AllUsers'      = $True
  'Get-WindowsDriver:Online'      = $True
  'Get-WindowsDriver:All'         = $True
  'Install-Module:Force'          = $True
  'Install-Module:Scope'          = 'AllUsers'
  'Remove-Item:Force'             = $True
  'Stop-Service:Force'            = $True
  'Invoke-WebRequest:Method'      = 'GET'
  'Clear-RecycleBin:Force'        = $True
}

. $PSScriptRoot\Script\Alias.ps1
. $PSScriptRoot\Script\Install.ps1

if ($null -ne $Env:SSH_CLIENT) {
  if ($HOME -eq $PWD.Path) {
    Set-Location code
  }

  . $PSScriptRoot\Script\Key.ps1
}
