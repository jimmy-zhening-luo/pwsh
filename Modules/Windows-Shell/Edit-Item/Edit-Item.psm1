New-Alias i Edit-Item
function Edit-Item {
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name')]
    [string]$ProfileName,
    [switch]$Window,
    [switch]$ReuseWindow,
    [string]$Location
  )

  $ArgumentList = @()

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $ArgumentList += $Location
    $Location = ''
  }

  if ($Path) {
    $Target = $Location ? (Join-Path $Location $Path) : $Path

    if (Test-Path -Path $Target) {
      $FullPath = Resolve-Path $Target |
        Select-Object -ExpandProperty Path
      $ArgumentList = , $FullPath + $ArgumentList
    }
    else {
      if (-not $Path.StartsWith('-')) {
        throw "Path '$Target' does not exist."
      }

      if (-not $Location) {
        $Location = '.'
      }

      $ArgumentList = $Location, $Path + $ArgumentList
    }
  }
  else {
    if (-not $Location) {
      $Location = '.'
    }

    $ArgumentList = , $Location + $ArgumentList
  }

  if ($env:SSH_CLIENT) {
    throw 'Cannot open VSCode from SSH session'
  }

  if ($ProfileName) {
    if (-not $ProfileName.StartsWith('-')) {
      $Window = $true

      $ArgumentList += '--profile'
    }

    $ArgumentList += $ProfileName
  }

  if ($Window) {
    $ArgumentList += '--new-window'
  }
  elseif ($ReuseWindow) {
    $ArgumentList += '--reuse-window'
  }

  if ($ArgumentList) {
    & code.cmd $ArgumentList
  }
  else {
    & code.cmd
  }
}

New-Alias i. Edit-Sibling
function Edit-Sibling {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -Location '..' @args
}

New-Alias i.. Edit-Relative
function Edit-Relative {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -Location '..\..' @args
}

New-Alias i~ Edit-Home
function Edit-Home {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -Location '~' @args
}

New-Alias ic Edit-Code
function Edit-Code {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -Location '~\code' @args
}

New-Alias i\ Edit-Drive
New-Alias i/ Edit-Drive
function Edit-Drive {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$Window,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -Location '\' @args
}
