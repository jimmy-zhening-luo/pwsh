#region Module
$Env:PSModulePath += ";$PSScriptRoot\Module"
#endregion


#region Data
$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1
[hashtable]$Private:CLASSES = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Class.psd1
#endregion


#region Script
[hashtable]$Private:ClassRoot = @{
  ClassRoot  = "$PSScriptRoot\Class"
  ModuleRoot = "$PSScriptRoot\Module"
}
. $PSScriptRoot\Script\Install.ps1 @ClassRoot @CLASSES
. $PSScriptRoot\Script\Alias.ps1
#endregion
