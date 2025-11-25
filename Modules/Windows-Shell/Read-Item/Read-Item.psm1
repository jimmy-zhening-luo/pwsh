New-Alias p Read-Item
function Read-Item {
  param(
    [Parameter(Position = 0)]
    [PathCompletions('.')]
    [string]$Path,
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
      Get-ChildItem -Path $Location $Argument
    }
    else {
      Get-ChildItem -Path $Location
    }
  }
}

New-Alias p. Read-Sibling
function Read-Sibling {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -Location '..' @args
}

New-Alias p.. Read-Relative
function Read-Relative {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('..\..')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -Location '..\..' @args
}

New-Alias p~ Read-Home
function Read-Home {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -Location '~' @args
}

New-Alias pc Read-Code
function Read-Code {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('~\code')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -Location '~\code' @args
}

New-Alias p\ Read-Drive
New-Alias p/ Read-Drive
function Read-Drive {
  param (
    [Parameter(Position = 0)]
    [PathCompletions('\')]
    [string]$Path
  )

  Read-Item @PSBoundParameters -Location '\' @args
}
