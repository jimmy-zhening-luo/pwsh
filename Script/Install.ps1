using namespace System.Collections.Generic

& {
  #region Solution
  $Root = Split-Path $PSScriptRoot
  $SourceRoot = "$Root\Class"
  $ModuleRoot = "$Root\Module"

  $DOTNET_SOLUTION = Import-PowerShellDataFile -Path $Root\Data\Class.psd1
  #endregion

  #region Installer
  function Install-PSProject {
    [CmdletBinding()]
    [OutputType([void])]
    param(
      [Parameter(
        Mandatory,
        ValueFromPipeline
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string[]]$Project,

      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Folder,

      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$SourceRoot,

      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$InstallRoot,

      [switch]$AppendProject
    )

    process {
      $BuildOutput = "$SourceRoot\$Folder\$Project\bin\Release\net9.0\$Project.dll"
  
      if (Test-Path $BuildOutput -PathType Leaf) {
        $InstallPath = $AppendProject ? (
          "$InstallRoot\$Project"
        ) : $InstallRoot
        $InstalledAssembly = "$InstallPath\$Project.dll"
  
        if (
          -not (
            Test-Path $InstalledAssembly -PathType Leaf
          ) -or (
            Get-FileHash -Path $InstalledAssembly -Algorithm MD5
          ).Hash -ne (
            Get-FileHash -Path $BuildOutput -Algorithm MD5
          ).Hash
        ) {
          Copy-Item -Path $BuildOutput -Destination $InstallPath -Force -ErrorAction Continue
        }
      }
      else {
        Write-Warning -Message "Project '$Folder\$Project' is not built, skipping."
      }
    }
  }
  #endregion

  #region Install
  $DOTNET_SOLUTION.Cmdlets |
    Install-PSProject -Folder Cmdlet -SourceRoot $SourceRoot -InstallRoot $ModuleRoot -AppendProject

  $DOTNET_SOLUTION.Types |
    Install-PSProject -Folder Type -SourceRoot $SourceRoot -InstallRoot $SourceRoot
  #endregion

  #region Add Type
  $DOTNET_SOLUTION.Types |
    Where-Object {
      Test-Path $SourceRoot\$PSItem.dll -PathType Leaf
    } |
    ForEach-Object {
      Add-Type -Path $SourceRoot\$PSItem.dll
    }
  #endregion
}
