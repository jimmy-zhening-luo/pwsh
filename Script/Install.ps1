& {
  $ROOT = Split-Path $PSScriptRoot
  $NAME = "Module"
  $SOURCE = "$ROOT\Class\$NAME"
  $INSTALL = "$ROOT\Module\$NAME"

  $BuildOutput = "$SOURCE\bin\Release\net9.0-windows\$NAME.dll"

  if (-not (Test-Path $BuildOutput -PathType Leaf)) {
    Write-Warning -Message "Class '$NAME' is not built."
  }

  $InstalledAssembly = "$INSTALL\$NAME.dll"

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

  if (Test-Path $InstalledAssembly -PathType Leaf) {
    Add-Type -Path $InstalledAssembly
  }
}
