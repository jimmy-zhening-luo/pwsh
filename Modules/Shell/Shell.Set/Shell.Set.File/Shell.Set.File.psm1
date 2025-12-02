New-Alias w Shell\Set-File
function Set-File {
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('.')]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value,
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

  $Target = $Location ? (
    Join-Path (Resolve-Path -Path $Location).Path $Path
  ) : (
    $Path ? $Path : $PWD.Path
  )

  $Write = @{
    Path  = $Target
    Value = $Value
  }

  Set-Content @Write @Local:args
}

New-Alias w. Shell\Set-FileSibling
function Set-FileSibling {
  param (
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('..')]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value
  )

  $Location = @{
    Location = $PWD | Split-Path
  }

  Set-File @PSBoundParameters @Location @args
}

New-Alias w.. Shell\Set-FileRelative
function Set-FileRelative {
  param (
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('..\..')]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value
  )

  $Location = @{
    Location = $PWD | Split-Path | Split-Path
  }

  Set-File @PSBoundParameters @Location @args
}

New-Alias w~ Shell\Set-FileHome
function Set-FileHome {
  param (
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('~')]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value
  )

  $Location = @{
    Location = $HOME
  }

  Set-File @PSBoundParameters @Location @args
}

New-Alias wc Shell\Set-FileCode
function Set-FileCode {
  param (
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('~\code')]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value
  )

  $Location = @{
    Location = "$HOME\code"
  }

  Set-File @PSBoundParameters @Location @args
}

New-Alias w/ Shell\Set-FileDrive
function Set-FileDrive {
  param (
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('\')]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value
  )

  $Location = @{
    Location = $PWD.Drive.Root
  }

  Set-File @PSBoundParameters @Location @args
}
