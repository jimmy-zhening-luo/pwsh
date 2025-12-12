using namespace System.IO
using namespace System.Collections.Generic

New-Alias p Get-File
function Get-File {

  [OutputType([string[]], [DirectoryInfo[]], [FileInfo[]])]

  param(

    [PathCompletions('.')]
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
      Path = Resolve-Path -Path $Target
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
      Path = $Location ? (Resolve-Path -Path $Location) : $PWD.Path
    }
    return Get-ChildItem @Directory @ArgumentList
  }
}

New-Alias p. Get-FileSibling
function Get-FileSibling {

  [OutputType([string[]])]

  param(

    [PathCompletions('..')]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $PWD | Split-Path
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias p.. Get-FileRelative
function Get-FileRelative {

  [OutputType([string[]])]

  param(

    [PathCompletions('..\..')]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $PWD | Split-Path | Split-Path
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias ph Get-FileHome
function Get-FileHome {

  [OutputType([string[]])]

  param(

    [PathCompletions('~')]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $HOME
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias pc Get-FileCode
function Get-FileCode {

  [OutputType([string[]])]

  param(

    [PathCompletions('~\code')]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = "$HOME\code"
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias p/ Get-FileDrive
function Get-FileDrive {

  [OutputType([string[]])]

  param(

    [PathCompletions('\')]
    [string]$Path

  )

  [hashtable]$Private:Location = @{
    Location = $PWD.Drive.Root
  }
  Get-File @PSBoundParameters @Location @args
}
