New-Alias p Read-Item
function Read-Item {
  param(
    [PathCompletions('.')]
    [string]$Path,
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

  if ($Path) {
    if (-not (Test-Path -Path $Target)) {
      throw "Path '$Target' does not exist."
    }

    $FullPath = Resolve-Path -Path $Target |
      Select-Object -ExpandProperty Path

    if (Test-Path -Path $FullPath -PathType Container) {
      if ($Argument) {
        Get-ChildItem -Path $FullPath $Argument
      }
      else {
        Get-ChildItem -Path $FullPath
      }
    }
    else {
      if ($Argument) {
        Get-Content -Path $FullPath $Argument
      }
      else {
        Get-Content -Path $FullPath
      }
    }
  }
  else {
    if ($Argument) {
      Get-ChildItem -Path $RootPath $Argument
    }
    else {
      Get-ChildItem -Path $RootPath
    }
  }
}

New-Alias p. Read-Sibling
function Read-Sibling {
  param (
    [PathCompletions('..')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath '..' @args
}

New-Alias p.. Read-Relative
function Read-Relative {
  param (
    [PathCompletions('..\..')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath '..\..' @args
}

New-Alias p~ Read-Home
function Read-Home {
  param (
    [PathCompletions('~')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath '~' @args
}

New-Alias pc Read-Code
function Read-Code {
  [OutputType([void])]
  param (
    [PathCompletions('~\code')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath '~\code' @args
}

New-Alias p\ Read-Drive
New-Alias p/ Read-Drive
function Read-Drive {
  param (
    [PathCompletions('\')]
      [string]$Path
  )

  Read-Item @PSBoundParameters -RootPath '\' @args
}
