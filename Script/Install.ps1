using namespace System.Collections.Generic

[CmdletBinding()]
param (

  [Parameter(
    Mandatory,
    Position = 0
  )]
  [ValidateNotNullOrWhiteSpace()]
  [string]$SourceRoot,

  [Parameter(
    Mandatory,
    Position = 1
  )]
  [ValidateNotNullOrWhiteSpace()]
  [string]$ModuleRoot,

  [Parameter(Mandatory)]
  [AllowEmptyCollection()]
  [ValidateNotNull()]
  [string[]]$Types,

  [Parameter(Mandatory)]
  [AllowEmptyCollection()]
  [ValidateNotNull()]
  [string[]]$Modules

)


#region Installer
function Install-PSProject {
  [CmdletBinding()]
  [OutputType([string])]
  param(
    [Parameter(Mandatory)]
    [ValidateNotNullOrWhiteSpace()]
    [string]$Project,

    [Parameter(Mandatory)]
    [ValidateNotNullOrWhiteSpace()]
    [string]$InstallPath
  )

  [string]$Private:BuildOutput = "$SourceRoot\$Project\bin\Release\net9.0\$Project.dll"

  if (Test-Path -Path $BuildOutput) {
    [string]$Private:InstalledAssembly = "$InstallPath\$Project.dll"

    if (
      -not (
        Test-Path -Path $InstalledAssembly -PathType Leaf
      ) -or (
        Get-FileHash -Path $InstalledAssembly
      ).Hash -ne (
        Get-FileHash -Path $BuildOutput
      ).Hash
    ) {
      [hashtable]$Private:Install = @{
        Path        = $BuildOutput
        Destination = $InstallPath
        Force       = $True
        ErrorAction = 'Continue'
      }
      Copy-Item @Install
    }
  }
  else {
    Write-Warning -Message "Project '$Project' is not built, skipping."
  }
}
#endregion


#region Install/Module
foreach ($Private:Module in $Modules) {
  [hashtable]$Private:ModuleDistro = @{
    Project     = $Module
    InstallPath = "$ModuleRoot\$Module"
  }
  Install-PSProject @ModuleDistro
}
#endregion


#region Install/Type
foreach ($Private:Type in $Types) {
  [hashtable]$Private:TypeDistro = @{
    Project     = $Type
    InstallPath = $SourceRoot
  }
  Install-PSProject @TypeDistro
}
#endregion


#region Load Type
foreach ($Private:Type in $Types) {
  [hashtable]$Private:TypeAssembly = @{
    Path = "$SourceRoot\$Type.dll"
  }

  if (Test-Path @TypeAssembly) {
    Add-Type @TypeAssembly
  }
}
#endregion
