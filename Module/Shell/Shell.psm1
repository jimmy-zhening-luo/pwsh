using namespace System.IO
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Clear-Line {

  [OutputType([void])]

  param(

    [PathCompletions(
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

  $Path = [TypedPath]::Normalize(
    $Path,
    '',
    $True
  )
  $Location = [TypedPath]::Normalize($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      if (
        [Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][TypedPath]::IsPathDescendantPattern
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
    $Path -match [regex][TypedPath]::IsPathTildeRootedPattern
  ) {
    $Path = $Path -replace [regex][TypedPath]::RemoveTildeRootPattern, ''

    if ($Location) {
      $Path = Join-Path $HOME $Path

      if (
        [Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][TypedPath]::IsPathDescendantPattern
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
  if (-not (Test-Path @Container)) {
    return $False
  }

  [string]$Private:FullLocation = (Resolve-Path -Path $Location).Path
  [string]$Private:FullPath = Join-Path $FullLocation $Path
  [bool]$Private:HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch [regex]'^\\*$'
  [bool]$Private:FileLike = $HasSubpath -and -not (
    $FullPath.EndsWith(
      [TypedPath]::PathSeparator
    ) -or $FullPath.EndsWith('..')
  )

  if (-not $HasSubpath) {
    return -not (
      $RequiresSubpath -or $File -or $New
    )
  }

  if ($File -and -not $FileLike) {
    return $False
  }

  [hashtable]$Private:Item = @{
    Path     = $FullPath
    PathType = $File ? 'Leaf' : 'Container'
  }
  if ($New) {
    return (Test-Path @Item -IsValid) -and -not (Test-Path @Item)
  }
  else {
    return Test-Path @Item
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
      $PSBoundParameters | ConvertTo-Json -EnumsAsStrings
    )
  }

  $Path = [TypedPath]::Normalize(
    $Path,
    [TypedPath]::PathSeparator,
    $True
  )
  $Location = [TypedPath]::Normalize($Location)

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
    $Path -match [regex][TypedPath]::IsPathTildeRootedPattern
  ) {
    $Path = $Path -replace [regex][TypedPath]::RemoveTildeRootPattern, ''

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
  [string]$Private:FullPath = Join-Path $FullLocation $Path

  if ($New) {
    return $FullPath
  }
  else {
    return [string](Resolve-Path -Path $FullPath -Force).Path
  }
}

New-Alias cl Clear-Line
