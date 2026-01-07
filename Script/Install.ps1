using namespace System.Collections.Generic

& {
  #region Solution
  $ROOT = Split-Path $PSScriptRoot
  $SLNX = Select-Xml -XPath Solution -Path $ROOT\Class\Class.slnx |
    Select-Object -ExpandProperty Node

  function Expand-PSProject {
    [CmdletBinding()]
    [OutputType([string[]])]
    param(
      [Parameter(Mandatory)]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Class
    )
    end {
      return $SLNX |
        Select-Xml -XPath (
          'Folder[@Name="/' + $Class + '/"]'
        ) |
        Select-Object -ExpandProperty Node |
        Select-Object -ExpandProperty Project |
        Select-Object -ExpandProperty Path |
        ForEach-Object {
          $PSItem.Substring(
            $PSItem.LastIndexOf([char]'/') + 1
          )
        } |
        ForEach-Object {
          $PSItem.Remove(
            $PSItem.Length - 7
          )
        }
    }
  }

  $Modules = Expand-PSProject -Class Module
  $Types = Expand-PSProject -Class Type
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
      $BuildOutput = "$ROOT\Class\$Class\$Project\bin\Release\net9.0\$Project.dll"

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
  $Modules |
    Install-PSProject -Class Module -AppendProject
  $Types |
    Install-PSProject -Class Type
  #endregion

  #region Add Type
  $Types |
    Where-Object {
      Test-Path $Root\Type\$PSItem.dll -PathType Leaf
    } |
    ForEach-Object {
      Add-Type -Path $Root\Type\$PSItem.dll
    }
  #endregion
}
