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
    [string]$Private:FullPath = $Location ? (
      Join-Path $Location $Path
    ) : $Path

    if (-not (Test-Path -Path $Private:FullPath)) {
      throw "Path '$Private:Target' does not exist."
    }

    if (Test-Path -Path $Private:FullPath -PathType Container) {
      return Get-ChildItem -Path $Private:FullPath @args
    }
    else {
      return Get-Content -Path $Private:FullPath @args
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

  Get-File -Path $Path -Location (Split-Path $PWD.Path) @args
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

  Get-File -Path $Path -Location ($PWD.Path | Split-Path | Split-Path) @args
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

  Get-File -Path $Path -Location $HOME @args
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

  Get-File -Path $Path -Location $REPO_ROOT @args
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

  Get-File -Path $Path -Location $PWD.Drive.Root @args
}

New-Alias p Get-File
New-Alias p. Get-FileSibling
New-Alias p.. Get-FileRelative
New-Alias ph Get-FileHome
New-Alias pc Get-FileCode
New-Alias p/ Get-FileDrive
