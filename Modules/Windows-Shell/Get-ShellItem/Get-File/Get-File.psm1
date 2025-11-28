New-Alias p Get-File
function Get-File {
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
      Path = Resolve-Path -Path $Target |
        Select-Object -ExpandProperty Path
    }

    if (Test-Path @FullPath -PathType Container) {
      Get-ChildItem @FullPath @Local:args
    }
    else {
      Get-Content @FullPath @Local:args
    }
  }
  else {
    $Directory = @{
      Path = $Location ? (
        Resolve-Path -Path $Location | Select-Object -ExpandProperty Path
      ) : $PWD.Path
    }

    Get-ChildItem @Directory @Local:args
  }
}

New-Alias p. Get-FileSibling
function Get-FileSibling {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path
  )

  $Location = @{
    Location = '..'
  }

  Get-File @PSBoundParameters @Location @args
}

New-Alias p.. Get-FileRelative
function Get-FileRelative {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path
  )

  $Location = @{
    Location = '..\..'
  }

  Get-File @PSBoundParameters @Location @args
}

New-Alias p~ Get-FileHome
function Get-FileHome {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path
  )

  $Location = @{
    Location = '~'
  }

  Get-File @PSBoundParameters @Location @args
}

New-Alias pc Get-FileCode
function Get-FileCode {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path
  )

  $Location = @{
    Location = '~\code'
  }

  Get-File @PSBoundParameters @Location @args
}

New-Alias p/ Get-FileDrive
function Get-FileDrive {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path
  )

  $Location = @{
    Location = '\'
  }

  Get-File @PSBoundParameters @Location @args
}
