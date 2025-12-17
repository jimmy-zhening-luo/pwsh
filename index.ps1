#region Module
$Env:PSModulePath += ";$PSScriptRoot\Module"
#endregion


#region Data
$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Parameter.psd1
[hashtable]$Private:DOTNET_SOLUTION = Import-PowerShellDataFile -Path $PSScriptRoot\Data\Class.psd1
#endregion


#region Script
[hashtable]$Private:Workspace = @{
  SourceRoot = "$PSScriptRoot\Class"
  ModuleRoot = "$PSScriptRoot\Module"
}
. $PSScriptRoot\Script\Install.ps1 @Workspace @DOTNET_SOLUTION
. $PSScriptRoot\Script\Alias.ps1
#endregion
