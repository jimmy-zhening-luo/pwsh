New-Alias p Shell\Get-File
function Get-File {
  [OutputType([string[]])]
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
    [string]$Location
  )

  $Local:args = $args

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Local:args = , $Location + $Local:args
    $Location = ''
  }

  if ($Path) {
    $Target = $Location ? (Join-Path $Location $Path) : $Path

    if (-not (Test-Path -Path $Target)) {
      throw "Path '$Target' does not exist."
    }

    $FullPath = @{
      Path = Resolve-Path -Path $Target
    }
    $Container = @{
      PathType = 'Container'
    }
    if (Test-Path @FullPath @Container) {
      Get-ChildItem @FullPath @Local:args
    }
    else {
      Get-Content @FullPath @Local:args
    }
  }
  else {
    $Directory = @{
      Path = $Location ? (Resolve-Path -Path $Location) : $PWD.Path
    }
    Get-ChildItem @Directory @Local:args
  }
}

New-Alias p. Shell\Get-FileSibling
function Get-FileSibling {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path
  )

  $Location = @{
    Location = $PWD | Split-Path
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

New-Alias p.. Shell\Get-FileRelative
function Get-FileRelative {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path
  )

  $Location = @{
    Location = $PWD | Split-Path | Split-Path
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

New-Alias p~ Shell\Get-FileHome
function Get-FileHome {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path
  )

  $Location = @{
    Location = $HOME
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

New-Alias pc Shell\Get-FileCode
function Get-FileCode {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path
  )

  $Location = @{
    Location = "$HOME\code"
  }
  Shell\Get-File @PSBoundParameters @Location @args
}

New-Alias p/ Shell\Get-FileDrive
function Get-FileDrive {
  [OutputType([string[]])]
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path
  )

  $Location = @{
    Location = $PWD.Drive.Root
  }
  Shell\Get-File @PSBoundParameters @Location @args
}
