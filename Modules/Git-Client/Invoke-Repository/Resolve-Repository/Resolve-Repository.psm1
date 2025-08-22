function Resolve-Repository {
  param([System.String]$Path)

  if (Test-Path (Join-Path $Path ".git") -PathType Container) {
    Resolve-Path $Path
  }
  else {
    $CodeSubpath = Join-Path $CODE ($Path -replace "^\.[\/\\]+", "")

    if (Test-Path (Join-Path $CodeSubpath ".git") -PathType Container) {
      Resolve-Path $CodeSubpath
    }
    else {
      $null
    }
  }
}
