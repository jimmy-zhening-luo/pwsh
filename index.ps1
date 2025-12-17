#region Module
$Env:PSModulePath += ";$PSScriptRoot\Module"
#endregion


#region Data
$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1
[hashtable]$Private:PROJECTS = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Class.psd1
#endregion


#region Script
[hashtable]$Private:Solution = @{
  SourceRoot = "$PSScriptRoot\Class"
  ModuleRoot = "$PSScriptRoot\Module"
}
. $PSScriptRoot\Script\Install.ps1 @Solution @PROJECTS
. $PSScriptRoot\Script\Alias.ps1
#endregion
