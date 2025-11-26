New-Alias w Write-Item
function Write-Item {
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
    Join-Path (
      Resolve-Path -Path $Location |
        Select-Object -ExpandProperty Path
    ) $Path
  ) : (
    $Path ? $Path : $PWD.Path
  )

  $Write = @{
    Path  = $Target
    Value = $Value
  }

  Set-Content @Write @Local:args
}

New-Alias w. Write-Sibling
function Write-Sibling {
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

  Write-Item @PSBoundParameters @Location @args
}

New-Alias w.. Write-Relative
function Write-Relative {
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

  Write-Item @PSBoundParameters @Location @args
}

New-Alias w~ Write-Home
function Write-Home {
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

  Write-Item @PSBoundParameters @Location @args
}

New-Alias wc Write-Code
function Write-Code {
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

  Write-Item @PSBoundParameters @Location @args
}

New-Alias w\ Write-Drive
New-Alias w/ Write-Drive
function Write-Drive {
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

  Write-Item @PSBoundParameters @Location @args
}
