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
    [string]$Location
  )

  $Argument = ''

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Argument = $Location
    $Location = ''
  }

  if (-not $Location) {
    $Location = '.'
  }

  $Location = Resolve-Path -Path $Location |
    Select-Object -ExpandProperty Path
  $Target = Join-Path $Location $Path

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

  Write-Item @PSBoundParameters -Location '..' @args
}

New-Alias w.. Write-Relative
function Write-Relative {
  param (
    [PathCompletions('..\..')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -Location '..\..' @args
}

New-Alias w~ Write-Home
function Write-Home {
  param (
    [PathCompletions('~')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -Location '~' @args
}

New-Alias wc Write-Code
function Write-Code {
  param (
    [PathCompletions('~\code')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -Location '~\code' @args
}

New-Alias w\ Write-Drive
New-Alias w/ Write-Drive
function Write-Drive {
  param (
    [PathCompletions('\')]
    [string[]]$Path,
    [Object[]]$Value
  )

  Write-Item @PSBoundParameters -Location '\' @args
}
