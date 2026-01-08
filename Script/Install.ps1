using namespace System.Collections.Generic

& {
  #region Solution
  $ROOT = Split-Path $PSScriptRoot
  $CACHE = "$ROOT\Class\Class.json"

  if (-not (Test-Path $CACHE -PathType Leaf)) {
    throw "Cache file not found: $CACHE"
  }

  $Solution = Get-Content -Path $CACHE |
    ConvertFrom-Json -AsHashtable
  $Modules = $Solution.Modules
  $Types = $Solution.Types

  if (-not $Modules -or -not $Types) {
    throw "Cache file is corrupted: $CACHE"
  }
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

    begin {
      $ClassRoot = "$ROOT\Class\$Class"
      $InstallLocation = "$Root\$Class"
    }

    process {
      $BuildOutput = "$ClassRoot\$Project\bin\Release\net9.0\$Project.dll"

      if (Test-Path $BuildOutput -PathType Leaf) {
        $InstallPath = $AppendProject ? "$InstallLocation\$Project" : $InstallLocation
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
