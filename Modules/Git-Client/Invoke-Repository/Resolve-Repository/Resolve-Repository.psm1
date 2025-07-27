function Resolve-Repository {
  param([string]$Path)

  if (
    Test-Path (
      Join-Path $Path ".git"
    )
  ) {
    Resolve-Path $Path
  }
  else {
    $CodeSubpath = Join-Path $code (
      $Path -replace "^\.[\/\\]+", ""
    )

    if (
      Test-Path (
        Join-Path $CodeSubpath ".git"
      )
    ) {
      Resolve-Path $CodeSubpath
    }
    else {
      $null
    }
  }
}
