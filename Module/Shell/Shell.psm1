using namespace System.Collections
using namespace System.Collections.Generic
using namespace System.Management.Automation
using namespace System.Management.Automation.Language
using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Clear-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Clear-Line {

  [OutputType([void])]

  param(

    [PathCompletions(
      '.',
      [PathItemType]::Any,
      $False,
      $False
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

  $Path = [TypedPath]::Format(
    $Path,
    '',
    $True
  )
  $Location = [TypedPath]::Format($Location)

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      if (
        [System.IO.Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][TypedPath]::IsPathDescendantPattern
      ) {
        $Path = [System.IO.Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex][TypedPath]::IsPathTildeRootedPattern) {
    $Path = $Path -replace [regex][TypedPath]::TildeRootPattern, ''

    if ($Location) {
      $Path = Join-Path $HOME $Path

      if (
        [System.IO.Path]::GetRelativePath(
          $Path,
          $Location
        ) -match [regex][TypedPath]::IsPathDescendantPattern
      ) {
        $Path = [System.IO.Path]::GetRelativePath(
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

  $Path = [TypedPath]::Format(
    $Path,
    '',
    $True
  )
  $Location = [TypedPath]::Format($Location)

  if ([System.IO.Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = [System.IO.Path]::GetRelativePath(
        $Location,
        $Path
      )
    }
    else {
      $Location = [System.IO.Path]::GetPathRoot($Path)
    }
  }
  elseif ($Path -match [regex][TypedPath]::IsPathTildeRootedPattern) {
    $Path = $Path -replace [regex][TypedPath]::TildeRootPattern, ''

    if ($Location) {
      $Path = [System.IO.Path]::GetRelativePath(
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
