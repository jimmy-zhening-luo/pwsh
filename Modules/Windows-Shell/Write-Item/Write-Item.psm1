New-Alias w Write-Item
function Write-Item {
  param(
    [PathCompletions('.')]
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [string]$Path,
    [Parameter(
      Mandatory,
      Position = 1
    )]
    [Object[]]$Value,
    [string]$RootPath
  )

  $Argument = ''

  if (
    $RootPath -and -not (
      Test-Path -Path $RootPath -PathType Container
    )
  ) {
    $Argument = $RootPath
    $RootPath = ''
  }

  if (-not $RootPath) {
    $RootPath = '.'
  }

  $RootPath = Resolve-Path -Path $RootPath |
    Select-Object -ExpandProperty Path
  $Target = Join-Path $RootPath $Path

  if ($Argument) {
    Set-Content -Path $Target -Value $Value $Argument @args
  }
  else {
    Set-Content -Path $Target -Value $Value @args
  }
}

New-Alias w. Write-Sibling
function Write-Sibling {
  param (
    [PathCompletions('..')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -RootPath '..' @args
}

New-Alias w.. Write-Relative
function Write-Relative {
  param (
    [PathCompletions('..\..')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -RootPath '..\..' @args
}

New-Alias w~ Write-Home
function Write-Home {
  param (
    [PathCompletions('~')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -RootPath '~' @args
}

New-Alias wc Write-Code
function Write-Code {
  param (
    [PathCompletions('~\code')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -RootPath '~\code' @args
}

New-Alias w\ Write-Drive
New-Alias w/ Write-Drive
function Write-Drive {
  param (
    [PathCompletions('\')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -RootPath '\' @args
}
