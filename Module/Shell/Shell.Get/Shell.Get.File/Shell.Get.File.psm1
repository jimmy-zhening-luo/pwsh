using namespace Completer.PathCompleter

function Get-File {
  [OutputType(
    [string[]],
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]]
  )]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path,

    [string]$Location
  )

  [string[]]$Private:ArgumentList = @()

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Private:ArgumentList += $Location
    $Location = ''
  }

  if ($Path) {
    [string]$Private:Target = $Location ? (Join-Path $Location $Path) : $Path

    if (-not (Test-Path -Path $Private:Target)) {
      throw "Path '$Private:Target' does not exist."
    }

    [hashtable]$Private:FullPath = @{
      Path = (Resolve-Path -Path $Private:Target).Path
    }
    [hashtable]$Private:Container = @{
      PathType = 'Container'
    }
    if (Test-Path @Private:FullPath @Private:Container) {
      return Get-ChildItem @Private:FullPath @args
    }
    else {
      return Get-Content @Private:FullPath @args
    }
  }
  else {
    [hashtable]$Private:Directory = @{
      Path = ($Location ? (Resolve-Path -Path $Location) : $PWD).Path
    }
    return Get-ChildItem @Private:Directory @Private:ArgumentList @args
  }
}

function Get-FileSibling {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      $null, $null
    )]
    [string]$Path
  )

  [hashtable]$Private:Location = @{
    Location = Split-Path $PWD.Path
  }
  Get-File @PSBoundParameters @Private:Location @args
}

function Get-FileRelative {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      $null, $null
    )]
    [string]$Path
  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Path | Split-Path | Split-Path
  }
  Get-File @PSBoundParameters @Private:Location @args
}

function Get-FileHome {

  [OutputType([string[]])]
  param(

    [PathLocationCompletions(
      '~',
      $null, $null
    )]
    [string]$Path
  )

  [hashtable]$Private:Location = @{
    Location = $HOME
  }
  Get-File @PSBoundParameters @Private:Location @args
}

function Get-FileCode {

  [OutputType([string[]])]
  param(

    [PathLocationCompletions(
      '~\code',
      $null, $null
    )]
    [string]$Path
  )

  [hashtable]$Private:Location = @{
    Location = $REPO_ROOT
  }
  Get-File @PSBoundParameters @Private:Location @args
}

function Get-FileDrive {

  [OutputType([string[]])]
  param(

    [PathLocationCompletions(
      '\',
      $null, $null
    )]
    [string]$Path
  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Drive.Root
  }
  Get-File @PSBoundParameters @Private:Location @args
}

New-Alias p Get-File
New-Alias p. Get-FileSibling
New-Alias p.. Get-FileRelative
New-Alias ph Get-FileHome
New-Alias pc Get-FileCode
New-Alias p/ Get-FileDrive
