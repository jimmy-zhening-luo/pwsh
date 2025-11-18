New-Alias i Edit-Item
function Edit-Item {
  [OutputType([void])]
  param(
    [PathCompletions('.')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('w')]
    [switch]$CreateWindow,
    [Alias('rw')]
    [switch]$ReuseWindow,
    [string]$RootPath
  )

  $ArgumentList = @()

  if (
    $RootPath -and -not (
      Test-Path -Path $RootPath -PathType Container
    )
  ) {
    $ArgumentList += $RootPath
    $RootPath = ''
  }

  if ($Path) {
    $Target = $RootPath ? (Join-Path $RootPath $Path) : $Path

    if (Test-Path -Path $Target) {
      $FullPath = Resolve-Path $Target |
        Select-Object -ExpandProperty Path
      $ArgumentList = , $FullPath + $ArgumentList
    }
    else {
      if (-not $Path.StartsWith('-')) {
        throw "Path '$Target' does not exist."
      }

      if (-not $RootPath) {
        $RootPath = '.'
      }

      $ArgumentList = $RootPath, $Path + $ArgumentList
    }
  }
  else {
    if (-not $RootPath) {
      $RootPath = '.'
    }

    $ArgumentList = , $RootPath + $ArgumentList
  }

  if ($env:SSH_CLIENT) {
    throw "Cannot open VSCode from SSH session"
  }

  if ($ProfileName) {
    if (-not $ProfileName.StartsWith('-')) {
      $CreateWindow = $true

      $ArgumentList += '--profile'
    }

    $ArgumentList += $ProfileName
  }

  if ($CreateWindow) {
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
    [PathCompletions('..')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$CreateWindow,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath '..' @args
}

New-Alias i.. Edit-Relative
function Edit-Relative {
  param (
    [PathCompletions('..\..')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$CreateWindow,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath '..\..' @args
}

New-Alias i~ Edit-Home
function Edit-Home {
  param (
    [PathCompletions('~')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$CreateWindow,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath '~' @args
}

New-Alias ic Edit-Code
function Edit-Code {
  param (
    [PathCompletions('~\code')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$CreateWindow,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath '~\code' @args
}

New-Alias i\ Edit-Drive
New-Alias i/ Edit-Drive
function Edit-Drive {
  param (
    [PathCompletions('\')]
    [string]$Path,
    [Alias('Name', 'pn')]
    [string]$ProfileName,
    [Alias('Window')]
    [switch]$CreateWindow,
    [Alias('rw')]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath '\' @args
}
