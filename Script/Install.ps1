using namespace System.Collections.Generic

& {
  #region Solution
  $Private:Root = Split-Path $PSScriptRoot
  $Private:SourceRoot = "$Private:Root\Class"
  $Private:ModuleRoot = "$Private:Root\Module"

  $Private:DOTNET_SOLUTION = Import-PowerShellDataFile -Path $Private:Root\Data\Class.psd1
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
      $Private:BuildOutput = "$SourceRoot\$Folder\$Project\bin\Release\net9.0\$Project.dll"
  
      if (Test-Path $Private:BuildOutput -PathType Leaf) {
        $Private:InstallPath = $AppendProject ? (
          "$InstallRoot\$Project"
        ) : $InstallRoot
        $Private:InstalledAssembly = "$Private:InstallPath\$Project.dll"
  
        if (
          -not (
            Test-Path $Private:InstalledAssembly -PathType Leaf
          ) -or (
            Get-FileHash -Path $Private:InstalledAssembly -Algorithm MD5
          ).Hash -ne (
            Get-FileHash -Path $Private:BuildOutput -Algorithm MD5
          ).Hash
        ) {
          Copy-Item -Path $Private:BuildOutput -Destination $Private:InstallPath -Force -ErrorAction Continue
        }
      }
      else {
        Write-Warning -Message "Project '$Folder\$Project' is not built, skipping."
      }
    }
  }
  #endregion

  #region Install
  $Private:DOTNET_SOLUTION.Cmdlets |
    Install-PSProject -Folder Cmdlet -SourceRoot $Private:SourceRoot -InstallRoot $Private:ModuleRoot -AppendProject

  $Private:DOTNET_SOLUTION.Types |
    Install-PSProject -Folder Type -SourceRoot $Private:SourceRoot -InstallRoot $Private:SourceRoot
  #endregion

  #region Add Type
  $Private:DOTNET_SOLUTION.Types |
    Where-Object {
      Test-Path $Private:SourceRoot\$PSItem.dll -PathType Leaf
    } |
    ForEach-Object {
      Add-Type -Path $Private:SourceRoot\$PSItem.dll
    }
  #endregion
}
