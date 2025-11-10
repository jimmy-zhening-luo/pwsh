New-Alias p Read-Item
function Read-Item {
  param(
    [PathCompletions(".", "")]
    [string]$Path,
    [string]$RootPath
  )

  $Argument = ''

  if ($RootPath) {
    if (Test-Path -Path $RootPath -PathType Container) {
      $RootPath = Resolve-Path -Path $RootPath |
        Select-Object -ExpandProperty Path
    }
    else {
      $Argument = $RootPath
      $RootPath = ''
    }
  }

  $FullPath = ($RootPath ? (Join-Path $RootPath $Path) : ($Path ? $Path : ''))

  if ($Path) {
    if (Test-Path -Path $FullPath) {
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
      throw "Path '$FullPath' does not exist."
    }
  }
  else {
    if ($FullPath) {
      if ($Argument) {
        Get-ChildItem -Path $FullPath $Argument
      }
      else {
        Get-ChildItem -Path $FullPath
      }
    }
    else {
      Get-ChildItem
    }
  }
}

New-Alias p. Read-Sibling
function Read-Sibling {
  param (
    [PathCompletions("..", "")]
    [string]$Path
  )

  Read-Item -Path $Path -RootPath ".."
}

New-Alias p.. Read-Relative
function Read-Relative {
  param (
    [PathCompletions("..\..", "")]
    [string]$Path
  )

  Read-Item -Path $Path -RootPath "..\.."
}

New-Alias p~ Read-Home
function Read-Home {
  param (
    [PathCompletions("~", "")]
    [string]$Path
  )

  Read-Item -Path $Path -RootPath "~"
}

New-Alias pc Read-Code
function Read-Code {
  [OutputType([void])]
  param (
    [PathCompletions("~\code", "")]
    [string]$Path
  )

  Read-Item -Path $Path -RootPath "~\code"
}

New-Alias p\ Read-Drive
New-Alias p/ Read-Drive
function Read-Drive {
  param (
    [PathCompletions("\", "")]
      [string]$Path
  )

  Read-Item -Path $Path -RootPath "\"
}
