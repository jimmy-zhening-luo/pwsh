using namespace System.Collections.Generic
using namespace Completer.PathCompleter

function Get-File {

  [OutputType([string[]], [System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]

  param(

    [PathCompletions(
      '.',
      $null, $null, $null
    )]
    [string]$Path,

    [string]$Location

  )

  $Private:ArgumentList = [List[string]]::new()
  if ($args) {
    $ArgumentList.AddRange(
      [List[string]]$args
    )
  }

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $ArgumentList.Insert(0, $Location)
    $Location = ''
  }

  if ($Path) {
    [string]$Private:Target = $Location ? (Join-Path $Location $Path) : $Path

    if (-not (Test-Path -Path $Target)) {
      throw "Path '$Target' does not exist."
    }

    [hashtable]$Private:FullPath = @{
      Path = (Resolve-Path -Path $Target).Path
    }
    [hashtable]$Private:Container = @{
      PathType = 'Container'
    }
    if (Test-Path @FullPath @Container) {
      return Get-ChildItem @FullPath @Private:args
    }
    else {
      return Get-Content @FullPath @Private:args
    }
  }
  else {
    [hashtable]$Private:Directory = @{
      Path = ($Location ? (Resolve-Path -Path $Location) : $PWD).Path
    }
    return Get-ChildItem @Directory @ArgumentList
  }
}

function Get-FileSibling {

  [OutputType([string[]])]

  param(

    [PathCompletions(
      '..',
      $null, $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = Split-Path $PWD.Path
  }
  Get-File @PSBoundParameters @Location @args
}

function Get-FileRelative {

  [OutputType([string[]])]

  param(

    [PathCompletions(
      '..\..',
      $null, $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Path | Split-Path | Split-Path
  }
  Get-File @PSBoundParameters @Location @args
}

function Get-FileHome {

  [OutputType([string[]])]

  param(

    [PathCompletions(
      '~',
      $null, $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $HOME
  }
  Get-File @PSBoundParameters @Location @args
}

function Get-FileCode {

  [OutputType([string[]])]

  param(

    [PathCompletions(
      '~\code',
      $null, $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = "$HOME\code"
  }
  Get-File @PSBoundParameters @Location @args
}

function Get-FileDrive {

  [OutputType([string[]])]

  param(

    [PathCompletions(
      '\',
      $null, $null, $null
    )]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Drive.Root
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias p Get-File
New-Alias p. Get-FileSibling
New-Alias p.. Get-FileRelative
New-Alias ph Get-FileHome
New-Alias pc Get-FileCode
New-Alias p/ Get-FileDrive
