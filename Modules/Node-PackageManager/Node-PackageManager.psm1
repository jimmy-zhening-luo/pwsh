function Resolve-NodeProject {
  param(
    [string]$Path
  )

  if (-not $Path) {
    $Path = $PWD.Path
  }

  $IsNode = @{
    Path     = Join-Path $Path 'package.json'
    PathType = 'Leaf'
  }

  if (Test-Path @IsNode) {
    $Prefix = (Resolve-Path $Path).Path

    $Prefix -eq $PWD.Path ? '' : $Prefix
  }
  else {
    throw "Path '$Path' is not a Node project directory."
  }
}
