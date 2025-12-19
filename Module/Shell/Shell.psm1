using namespace System.IO
using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Clear-Line {

  [OutputType([void])]

  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path

  )

  if ($Path -or $args) {
    Clear-Content @PSBoundParameters @args
  }
  else {
    Clear-Host
  }
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

  $Path = [Canonicalizer]::Normalize(
    $Path,
    '',
    $True
  )
  $Location = [Canonicalizer]::Normalize($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      if (
        [Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][Canonicalizer]::IsPathDescendantPattern
      ) {
        $Path = [Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif (
    $Path -match [regex][Canonicalizer]::IsPathTildeRootedPattern
  ) {
    $Path = $Path -replace [regex][Canonicalizer]::RemoveTildeRootPattern, ''

    if ($Location) {
      $Path = Join-Path $HOME $Path

      if (
        [Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][Canonicalizer]::IsPathDescendantPattern
      ) {
        $Path = [Path]::GetRelativePath(
          $Location,
          $Path
        )
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

  [hashtable]$Private:Container = @{
    Path     = $Location
    PathType = 'Container'
  }
  if (-not (Test-Path @Private:Container)) {
    return $False
  }

  [string]$Private:FullLocation = (Resolve-Path -Path $Location).Path
  [string]$Private:FullPath = Join-Path $Private:FullLocation $Path
  [bool]$Private:HasSubpath = $Private:FullPath.Substring($Private:FullLocation.Length) -notmatch [regex]'^\\*$'
  [bool]$Private:FileLike = $Private:HasSubpath -and -not (
    $Private:FullPath.EndsWith(
      [Canonicalizer]::PathSeparator
    ) -or $Private:FullPath.EndsWith('..')
  )

  if (-not $Private:HasSubpath) {
    return -not (
      $RequiresSubpath -or $File -or $New
    )
  }

  if ($File -and -not $FileLike) {
    return $False
  }

  [hashtable]$Private:Item = @{
    Path     = $Private:FullPath
    PathType = $File ? 'Leaf' : 'Container'
  }
  if ($New) {
    return (Test-Path @Private:Item -IsValid) -and -not (Test-Path @Private:Item)
  }
  else {
    return Test-Path @Private:Item
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
    throw "Invalid path '$Path': " + (
      $PSBoundParameters | ConvertTo-Json -EnumsAsStrings -Depth 6
    )
  }

  $Path = [Canonicalizer]::Normalize(
    $Path,
    [Canonicalizer]::PathSeparator,
    $True
  )
  $Location = [Canonicalizer]::Normalize($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = [Path]::GetRelativePath(
        $Location,
        $Path
      )
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif (
    $Path -match [regex][Canonicalizer]::IsPathTildeRootedPattern
  ) {
    $Path = $Path -replace [regex][Canonicalizer]::RemoveTildeRootPattern, ''

    if ($Location) {
      $Path = [Path]::GetRelativePath(
        $Location,
        (Join-Path $HOME $Path)
      )
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  [string]$Private:FullLocation = (Resolve-Path -Path $Location).Path
  [string]$Private:FullPath = Join-Path $Private:FullLocation $Path

  if ($New) {
    return $Private:FullPath
  }
  else {
    return [string](Resolve-Path -Path $Private:FullPath -Force).Path
  }
}

New-Alias cl Clear-Line
