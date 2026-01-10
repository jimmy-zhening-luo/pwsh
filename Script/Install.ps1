using namespace System.Collections.Generic

& {
  $ROOT = Split-Path $PSScriptRoot

  function Test-PSAssembly {
    [CmdletBinding()]
    [OutputType([bool])]
    param(
      [Parameter(
        Mandatory,
        Position = 0
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Source,

      [Parameter(
        Mandatory,
        Position = 1
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Destination
    )

    end {
      return (
        -not (
          Test-Path $Destination -PathType Leaf
        ) -or (
          Get-FileHash -Path $Destination -Algorithm MD5
        ).Hash -ne (
          Get-FileHash -Path $Source -Algorithm MD5
        ).Hash
      )
    }
  }

  function Install-PSAssembly {
    [CmdletBinding()]
    [OutputType([void])]
    param(
      [Parameter(
        Mandatory,
        Position = 0
      )]
      [ValidateNotNullOrWhiteSpace()]
      [string]$Class
    )

    end {
      $BuildOutput = "$ROOT\Class\$Class\bin\Release\net9.0\$Class.dll"

      if (-not (Test-Path $BuildOutput -PathType Leaf)) {
        Write-Warning -Message "Class '$Class' is not built, skipping."
      }

      $InstallPath = "$ROOT\Module\$Class"
      $InstalledAssembly = "$InstallPath\$Project.dll"

      if (Test-PSAssembly $BuildOutput $InstalledAssembly) {
        Copy-Item -Path $BuildOutput -Destination $InstallPath -Force -ErrorAction Continue
      }
    }
  }

  Install-PSAssembly -Class Module -Module
  Install-PSAssembly -Class Completer

  $InstalledCompleter = "$Root\Module\Completer\Completer.dll"
  if (Test-Path $InstalledCompleter -PathType Leaf) {
    Add-Type -Path $InstalledCompleter
  }
}
