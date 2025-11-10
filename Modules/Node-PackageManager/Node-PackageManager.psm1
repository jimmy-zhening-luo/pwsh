function Resolve-NodeProject {
  [OutputType([string])]
  param([string]$Path = ".")

  $PKG = "package.json"
  $PkgPath = (Join-Path $Path $PKG)

  if (Test-Path $PkgPath -PathType Leaf) {
    Resolve-Path $Path |
      Select-Object -ExpandProperty Path
  }
  else { '' }
}
