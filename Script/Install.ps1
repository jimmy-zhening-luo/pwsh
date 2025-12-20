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

    if (Test-Path -Path $Private:BuildOutput -PathType Leaf) {
      [string]$Private:InstalledAssembly = "$InstallPath\$Project.dll"

      if (
        -not (
          Test-Path -Path $Private:InstalledAssembly -PathType Leaf
        ) -or (
          Get-FileHash -Path $Private:InstalledAssembly -Algorithm MD5
        ).Hash -ne (
          Get-FileHash -Path $Private:BuildOutput -Algorithm MD5
        ).Hash
      ) {
        Copy-Item -Path $Private:BuildOutput -Destination $InstallPath -Force -ErrorAction Continue
      }
    }
    else {
      Write-Warning -Message "Project '$Folder\$Project' is not built, skipping."
    }
  }
  #endregion


  #region Install
  foreach ($Private:BinaryModule in $Private:Cmdlets) {
    Install-PSProject -Project $Private:BinaryModule -Folder Cmdlet -SourceRoot $Private:SourceRoot -InstallPath $Private:ModuleRoot\$Private:BinaryModule
  }

  foreach ($Private:Type in $Private:Types) {
    Install-PSProject -Project $Private:Type -Folder Type -SourceRoot $Private:SourceRoot -InstallPath $Private:SourceRoot
  }
  #endregion


  #region Add Type
  foreach ($Private:Type in $Private:Types) {
    [string]$Private:TypeAssembly = "$Private:SourceRoot\$Private:Type.dll"

    if (Test-Path -Path $Private:TypeAssembly -PathType Leaf) {
      Add-Type -Path $Private:TypeAssembly
    }
  }
  #endregion
}
