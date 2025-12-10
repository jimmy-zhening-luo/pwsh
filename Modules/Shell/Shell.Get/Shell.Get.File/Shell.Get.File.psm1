New-Alias p Get-File
function Get-File {
  [OutputType([string[]])]
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
    [string]$Location
  )

  $Private:args = $args

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Private:args = , $Location + $Private:args
    $Location = ''
  }

  if ($Path) {
    $Private:Target = $Location ? (Join-Path $Location $Path) : $Path

    if (-not (Test-Path -Path $Target)) {
      throw "Path '$Target' does not exist."
    }

    $Private:FullPath = @{
      Path = Resolve-Path -Path $Target
    }
    $Private:Container = @{
      PathType = 'Container'
    }
    if (Test-Path @FullPath @Container) {
      Get-ChildItem @FullPath @Private:args
    }
    else {
      Get-Content @FullPath @Private:args
    }
  }
  else {
    $Private:Directory = @{
      Path = $Location ? (Resolve-Path -Path $Location) : $PWD.Path
    }
    Get-ChildItem @Directory @Local:args
  }
}

New-Alias p. Get-FileSibling
function Get-FileSibling {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path
  )

  $Private:Location = @{
    Location = $PWD | Split-Path
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias p.. Get-FileRelative
function Get-FileRelative {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path
  )

  $Private:Location = @{
    Location = $PWD | Split-Path | Split-Path
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias ph Get-FileHome
function Get-FileHome {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path
  )

  $Private:Location = @{
    Location = $HOME
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias pc Get-FileCode
function Get-FileCode {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path
  )

  $Private:Location = @{
    Location = "$HOME\code"
  }
  Get-File @PSBoundParameters @Location @args
}

New-Alias p/ Get-FileDrive
function Get-FileDrive {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path
  )

  $Private:Location = @{
    Location = $PWD.Drive.Root
  }
  Get-File @PSBoundParameters @Location @args
}
