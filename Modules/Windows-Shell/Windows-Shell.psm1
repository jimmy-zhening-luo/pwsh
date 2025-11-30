function Format-Path {
  param(
    [string]$Path,
    [switch]$Leading,
    [switch]$Trailing
  )

  $TrimmedPath = $Path -replace '[\\\/]+', '\'

  if ($Leading) {
    $TrimmedPath = $Path -replace '^(?>\.\\+)', ''
  }

  if ($Trailing) {
    $TrimmedPath = $Path -replace '\\+$', ''
  }

  $TrimmedPath
}

function Trace-RelativePath {
  param(
    [string]$Path,
    [string]$Location
  )
  [System.IO.Path]::GetRelativePath($Path, $Location) -match '^[.\\]*$'
}

function Merge-RelativePath {
  param(
    [string]$Path,
    [string]$Location
  )

  [System.IO.Path]::GetRelativePath($Location, $Path)
}

function Test-Item {
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  $Location = Format-Path -Path $Location
  $Path = Format-Path -Path $Path -Leading

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Relative = @{
        Path = $Path
        Location = $Location
      }

      if (Trace-RelativePath @Relative) {
        $Path = Merge-RelativePath @Relative
      }
      else {
        return $False
      }
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^~\\*', ''

    if ($Location) {
      $Relative = @{
        Path = Join-Path $HOME $Path
        Location = $Location
      }

      if (Trace-RelativePath @Relative) {
        $Path = Merge-RelativePath @Relative
      }
      else {
        return $False
      }
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  if (-not (Test-Path -Path $Location -PathType Container)) {
    return $False
  }

  $FullLocation = (Resolve-Path -Path $Location).Path
  $FullPath = Join-Path $FullLocation $Path
  $HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch '^\\*$'
  $FileLike = $HasSubpath -and -not (
    $FullPath.EndsWith('\') -or $FullPath.EndsWith('..')
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

  $Location = Format-Path -Path $Location
  $Path = Format-Path -Path $Path -Leading

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = Merge-RelativePath -Path $Path -Location $Location
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^~\\*', ''

    if ($Location) {
      $Path = Merge-RelativePath -Path (
        Join-Path $HOME $Path
      ) -Location $Location
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  $FullLocation = (Resolve-Path -Path $Location).Path
  $FullPath = Join-Path $FullLocation $Path

  $New ? $FullPath : (
    Resolve-Path -Path $FullPath -Force
  ).Path
}
