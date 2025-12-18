using namespace System.Collections.Generic

& {
  #region Solution
  $Private:Root = Split-Path $PSScriptRoot
  $Private:SourceRoot = "$Private:Root\Class"
  $Private:ModuleRoot = "$Private:Root\Module"

  [hashtable]$Private:DOTNET_SOLUTION = Import-PowerShellDataFile -Path $Private:Root\Data\Class.psd1
  [string[]]$Private:Cmdlets = [string[]]$Private:DOTNET_SOLUTION.Cmdlets
  [string[]]$Private:Types = [string[]]$Private:DOTNET_SOLUTION.Types
  #endregion


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
      [string]$Folder,

      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$SourceRoot,

      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$InstallPath
    )

    [string]$Private:BuildOutput = "$SourceRoot\$Folder\$Project\bin\Release\net9.0\$Project.dll"

    if (Test-Path -Path $Private:BuildOutput) {
      [string]$Private:InstalledAssembly = "$InstallPath\$Project.dll"

      if (
        -not (
          Test-Path -Path $Private:InstalledAssembly -PathType Leaf
        ) -or (
          Get-FileHash -Path $Private:InstalledAssembly
        ).Hash -ne (
          Get-FileHash -Path $Private:BuildOutput
        ).Hash
      ) {
        [hashtable]$Private:Install = @{
          Path        = $Private:BuildOutput
          Destination = $InstallPath
          Force       = $True
          ErrorAction = 'Continue'
        }
        Copy-Item @Private:Install
      }
    }
    else {
      Write-Warning -Message "Project '$Folder\$Project' is not built, skipping."
    }
  }
  #endregion


  #region Install
  foreach ($Private:BinaryModule in $Private:Cmdlets) {
    [hashtable]$Private:BinaryModuleManifest = @{
      Project     = $Private:BinaryModule
      Folder      = 'Cmdlet'
      SourceRoot  = $Private:SourceRoot
      InstallPath = "$Private:ModuleRoot\$Private:BinaryModule"
    }
    Install-PSProject @Private:BinaryModuleManifest
  }

  foreach ($Private:Type in $Private:Types) {
    [hashtable]$Private:TypeManifest = @{
      Project     = $Private:Type
      Folder      = 'Type'
      SourceRoot  = $Private:SourceRoot
      InstallPath = $Private:SourceRoot
    }
    Install-PSProject @Private:TypeManifest
  }
  #endregion


  #region Add Type
  foreach ($Private:Type in $Private:Types) {
    [hashtable]$Private:TypeAssembly = @{
      Path = "$Private:SourceRoot\$Private:Type.dll"
    }

    if (Test-Path @Private:TypeAssembly) {
      Add-Type @Private:TypeAssembly
    }
  }
  #endregion
}
