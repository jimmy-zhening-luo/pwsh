function Resolve-Repository {
  param([System.String]$Path)

  if (Test-Path (Join-Path $Path ".git")) {
    Resolve-Path $Path
  }
  else {
    $CodeSubpath = Join-Path $CODE ($Path -replace "^\.[\/\\]+", "")

    if (Test-Path (Join-Path $CodeSubpath ".git")) {
      Resolve-Path $CodeSubpath
    }
    else {
      $null
    }
  }
}
