function Test-Item {
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  if (-not $Location) {
    $Location = $Path -match '^~[\/\\]' ? $HOME : $PWD.Path
  }

  if (-not (Test-Path -Path $Location -PathType Container)) {
    return $False
  }

  $FullLocation = Resolve-Path -Path $Location |
    Select-Object -ExpandProperty Path

  if ($Path -match '^~(?>[\/\\]|$)') {
    if ($FullLocation -replace '\\*$', '' -eq $HOME -replace '\\*$', '') {
      $Path = $Path -replace '^~[\/\\]+', ''
    }
    else {
      return $False
    }
  }

  $FullPath = Join-Path $FullLocation ($Path -replace '^\.[\/\\]+', '')
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
    PathType = $File ? 'Leaf' : 'Container'
  }

  if ($New) {
    (Test-Path @Item -IsValid) -and -not (Test-Path @Item)
  }
  else {
    Test-Path @Item
  }
}

function Resolve-Item {
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  if (-not (Test-Item @PSBoundParameters)) {
    throw "Path '$Path' fails to meet criteria: " + ($PSBoundParameters | ConvertTo-Json)
  }

  if (-not $Location) {
    $Location = $Path -match '^~(?>[\/\\]|$)' ? $HOME : $PWD.Path
  }

  $FullLocation = Resolve-Path -Path $Location |
    Select-Object -ExpandProperty Path
  $FullPath = Join-Path $FullLocation ($Path -replace '^~[\/\\]+', '' -replace '^\.[\/\\]+')

  if ($New) {
    $FullPath
  }
  else {
    Resolve-Path -Path $FullPath |
      Select-Object -ExpandProperty Path
  }
}
