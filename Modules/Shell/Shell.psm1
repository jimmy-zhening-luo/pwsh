function Format-Path {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Separator,
    [switch]$LeadingRelative,
    [switch]$Trailing
  )

  $AlignedPath = $Path -replace '[\\\/]', '\'
  $TrimmedPath = $AlignedPath -replace '(?<!^)(?>\\+)', '\'

  if ($LeadingRelative) {
    $TrimmedPath = $TrimmedPath -replace '^\.(?>\\+)', ''
  }

  if ($Trailing) {
    $TrimmedPath = $TrimmedPath -replace '(?>\\+)$', ''
  }

  $Separator -and $Separator -ne '\' ? $TrimmedPath -replace '\\', $Separator : $TrimmedPath
}

function Trace-RelativePath {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location
  )

  [System.IO.Path]::GetRelativePath($Path, $Location) -match '^(?>[.\\]*)$'
}

function Merge-RelativePath {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location
  )

  [System.IO.Path]::GetRelativePath($Location, $Path)
}

function Test-Item {
  [OutputType([bool])]
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  $Path = Shell\Format-Path -Path $Path -LeadingRelative
  $Location = Shell\Format-Path -Path $Location

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Relative = @{
        Path     = $Path
        Location = $Location
      }
      if (Shell\Trace-RelativePath @Relative) {
        $Path = Shell\Merge-RelativePath @Relative
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
    $Path = $Path -replace '^~(?>\\*)', ''

    if ($Location) {
      $Relative = @{
        Path     = Microsoft.PowerShell.Management\Join-Path $HOME $Path
        Location = $Location
      }
      if (Shell\Trace-RelativePath @Relative) {
        $Path = Shell\Merge-RelativePath @Relative
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
    $Location = (Microsoft.PowerShell.Management\Get-Location).Path
  }

  $Container = @{
    Path     = $Location
    PathType = 'Container'
  }
  if (-not (Microsoft.PowerShell.Management\Test-Path @Container)) {
    return $False
  }

  $FullLocation = (Microsoft.PowerShell.Management\Resolve-Path -Path $Location).Path
  $FullPath = Microsoft.PowerShell.Management\Join-Path $FullLocation $Path
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
    Path     = $FullPath
    PathType = $File ? 'Leaf' : 'Container'
  }
  if ($New) {
    (Microsoft.PowerShell.Management\Test-Path @Item -IsValid) -and -not (Microsoft.PowerShell.Management\Test-Path @Item)
  }
  else {
    Microsoft.PowerShell.Management\Test-Path @Item
  }
}

function Resolve-Item {
  [OutputType([string])]
  param(
    [string]$Path,
    [string]$Location,
    [switch]$File,
    [switch]$New,
    [switch]$RequireSubpath
  )

  if (-not (Test-Item @PSBoundParameters)) {
    throw "Invalid path '$Path': " + ($PSBoundParameters | Microsoft.PowerShell.Utility\ConvertTo-Json -EnumsAsStrings)
  }

  $Path = Shell\Format-Path -Path $Path -LeadingRelative
  $Location = Shell\Format-Path -Path $Location

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = Shell\Merge-RelativePath -Path $Path -Location $Location
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match '^~(?=\\|$)') {
    $Path = $Path -replace '^~(?>\\*)', ''

    if ($Location) {
      $Path = Shell\Merge-RelativePath -Path (
        Microsoft.PowerShell.Management\Join-Path $HOME $Path
      ) -Location $Location
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = (Microsoft.PowerShell.Management\Get-Location).Path
  }

  $FullLocation = (Microsoft.PowerShell.Management\Resolve-Path -Path $Location).Path
  $FullPath = Microsoft.PowerShell.Management\Join-Path $FullLocation $Path

  $New ? $FullPath : (
    Microsoft.PowerShell.Management\Resolve-Path -Path $FullPath -Force
  ).Path
}
