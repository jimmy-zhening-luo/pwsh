function Test-Item {
  param(
    [string]$Path,
    [string]$Location = $PWD.Path,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  if (-not (Test-Path -Path $Location -PathType Container)) {
    return $False;
  }

  $FullLocation = Resolve-Path -Path $Location |
    Select-Object -ExpandProperty Path
  $FullPath = Join-Path $FullLocation (
    $Path -replace '^\.[\/\\]+', ''
  )
  $HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch '^\\*$'
  $FileLike = $HasSubpath -and -not (
    $FullPath.EndsWith('\') -or $FullPath.EndsWith('\..')
  )

  if (-not $HasSubpath) {
    return -not (
      $RequiresSubpath -or $File -or $New
    )
  }

  if ($File -and -not $FileLike) {
    return $False
  }

  $Item = @{
    Path = $FullPath
    PathType = $File ? 'File' : 'Container'
  }

  $New ? (
    (
      Test-Path @Item -IsValid
    ) -and -not (
      Test-Path @Item
    )
  ) : (
    Test-Path @Item
  )
}

function Resolve-Item {
  param(
    [string]$Path,
    [string]$Location = $PWD.Path,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  if (-not (Test-Path @PSBoundParameters))
    throw "Path '$Path' fails to meet criteria: " + ($PSBoundParameters | ConvertTo-Json)

  $FullLocation = Resolve-Path -Path $Location |
    Select-Object -ExpandProperty Path
  $Item = {
    Path = Join-Path $FullLocation (
      $Path -replace '^\.[\/\\]+', ''
    )
  }

  Resolve-Path @Item
}
