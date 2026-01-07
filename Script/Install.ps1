using namespace System.Collections.Generic

& {
  #region Solution
  $Root = Split-Path $PSScriptRoot

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
      [string]$Class,

      [switch]$AppendProject
    )

    process {
      $BuildOutput = "$Root\Class\$Class\$Project\bin\Release\net9.0\$Project.dll"

      if (Test-Path $BuildOutput -PathType Leaf) {
        $InstallPath = "$Root\$Class" + (
          $AppendProject ? "\$Project" : ''
        )
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
        Write-Warning -Message "Project '$Class\$Project' is not built, skipping."
      }
    }
  }
  #endregion

  #region Install
  $DOTNET_SOLUTION.Modules |
    Install-PSProject -Class Module -AppendProject

  $DOTNET_SOLUTION.Types |
    Install-PSProject -Class Type
  #endregion

  #region Add Type
  $DOTNET_SOLUTION.Types |
    Where-Object {
      Test-Path $Root\Type\$PSItem.dll -PathType Leaf
    } |
    ForEach-Object {
      Add-Type -Path $Root\Type\$PSItem.dll
    }
  #endregion
}
