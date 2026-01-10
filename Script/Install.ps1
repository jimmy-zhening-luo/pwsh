using namespace System.Collections.Generic

& {
  $ROOT = Split-Path $PSScriptRoot
  $SOURCE_ROOT = "$ROOT\Class"
  $INSTALL_ROOT = "$ROOT\Module"

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
      $BuildOutput = "$SOURCE_ROOT\$Class\bin\Release\net9.0\$Class.dll"

      if (-not (Test-Path $BuildOutput -PathType Leaf)) {
        Write-Warning -Message "Class '$Class' is not built, skipping."
      }

      $InstallLocation = "$INSTALL_ROOT\$Class"
      $InstalledAssembly = "$InstallLocation\$Class.dll"

      if (
        -not (
          Test-Path $InstalledAssembly -PathType Leaf
        ) -or (
          Get-FileHash -Path $InstalledAssembly -Algorithm MD5
        ).Hash -ne (
          Get-FileHash -Path $BuildOutput -Algorithm MD5
        ).Hash
      ) {
        Copy-Item -Path $BuildOutput -Destination $InstallLocation -Force -ErrorAction Continue
      }
    }
  }

  Install-PSAssembly -Class Module
  Install-PSAssembly -Class Completer

  $InstalledCompleter = "$INSTALL_ROOT\Completer\Completer.dll"
  if (Test-Path $InstalledCompleter -PathType Leaf) {
    Add-Type -Path $InstalledCompleter
  }
}
