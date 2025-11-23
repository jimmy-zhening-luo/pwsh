function Resolve-NodeProject {
  [OutputType([string])]
  param([string]$Path)

  if (-not $Path) {
    $Path = $PWD.Path
  }

  $Criteria = @{
    Path = Join-Path $Path 'package.json'
    PathType = 'Leaf'
  }

  if (Test-Path @Criteria) {
    Resolve-Path $Path |
      Select-Object -ExpandProperty Path
  }
  else { '' }
}

