& {
  $CLASS = 'Module'
  $ROOT = Split-Path $PSScriptRoot
  $DIST = "$ROOT\Class\bin\Release\net9.0-windows\$CLASS.dll"
  $MODULE = "$ROOT\Module\$CLASS"
  $ASSEMBLY = "$MODULE\$CLASS.dll"

  $Exists = Test-Path $ASSEMBLY -PathType Leaf

  if (Test-Path $DIST -PathType Leaf) {
    if (
      !$Exists -or (
        Get-FileHash -Path $ASSEMBLY -Algorithm MD5
      ).Hash -ne (
        Get-FileHash -Path $DIST -Algorithm MD5
      ).Hash
    ) {
      Copy-Item -Path $DIST -Destination $MODULE -Force -ErrorAction Continue
    }
  }
  else {
    Write-Warning -Message 'Module assembly is not built.'
  }

  if ($Exists) {
    Add-Type -Path $ASSEMBLY
  }
}
