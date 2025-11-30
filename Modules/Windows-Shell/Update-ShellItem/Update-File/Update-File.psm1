New-Alias w Update-File
function Update-File {
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

New-Alias w. Update-FileSibling
function Update-FileSibling {
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
    Location = '..'
  }

  Update-File @PSBoundParameters @Location @args
}

New-Alias w.. Update-FileRelative
function Update-FileRelative {
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
    Location = '..\..'
  }

  Update-File @PSBoundParameters @Location @args
}

New-Alias w~ Update-FileHome
function Update-FileHome {
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
    Location = '~'
  }

  Update-File @PSBoundParameters @Location @args
}

New-Alias wc Update-FileCode
function Update-FileCode {
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
    Location = '~\code'
  }

  Update-File @PSBoundParameters @Location @args
}

New-Alias w/ Update-FileDrive
function Update-FileDrive {
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
    Location = '\'
  }

  Update-File @PSBoundParameters @Location @args
}
