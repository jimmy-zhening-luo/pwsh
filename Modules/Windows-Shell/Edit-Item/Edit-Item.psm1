New-Alias i Edit-Item
<#
.SYNOPSIS
Edit a file in Visual Studio Code.
.DESCRIPTION
This function is an alias for the Visual Studio Code command line interface, 'code.cmd'.
.LINK
https://code.visualstudio.com/docs/configure/command-line
#>
function Edit-Item {
  [OutputType([void])]
  param(
    [PathCompletions(".", "")]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [switch]$Window,
    [Alias("rw")]
    [switch]$ReuseWindow,
    [string]$RootPath
  )

  $ArgumentList = @()

  if ($RootPath) {
    if (Test-Path -Path $RootPath -PathType Container) {
        $RootPath = Resolve-Path -Path $RootPath |
        Select-Object -ExpandProperty Path
    }
    else {
      $ArgumentList += $RootPath
      $RootPath = ''
    }
  }
  $FullPath = ($RootPath ? (Join-Path $RootPath $Path) : ($Path ? $Path : ''))

  if ($Path) {
    if (Test-Path -Path $FullPath) {
      $ArgumentList = , (
        Resolve-Path -Path $FullPath |
          Select-Object -ExpandProperty Path
      ) + $ArgumentList
    }
    else {
      if (-not $Path.StartsWith("-")) {
        throw "Path '$FullPath' does not exist."
      }

      $FullPath = ($RootPath ? ($RootPath) : ".")
      $ArgumentList = $FullPath, $Path + $ArgumentList
    }
  }
  else {
    if ($FullPath) {
      $ArgumentList = , $FullPath + $ArgumentList
    }
  }

  if ($env:SSH_CLIENT) {
    if ($FullPath -and (Test-Path -Path $FullPath -PathType   Container)) {
      Set-Location -Path $FullPath
    }
    else {
      Read-Item -Path $FullPath
    }
  }
  else {
    if ($ProfileName) {
      if (-not $ProfileName.StartsWith("-")) {
        $Window = $true

        $ArgumentList += "--profile"
      }

      $ArgumentList += $ProfileName
    }

    if ($Window) {
      $ArgumentList += "--new-window"
    }
    elseif ($ReuseWindow) {
      $ArgumentList += "--reuse-window"
    }

    if ($CommandArguments) {
      & code.cmd $CommandArguments @args
    }
    else {
      & code.cmd @args
    }
  }
}

New-Alias i. Edit-Sibling
function Edit-Sibling {
  param (
    [PathCompletions("..", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Window,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath ".." @args
}

New-Alias i.. Edit-Relative
function Edit-Relative {
  param (
    [PathCompletions("..\..", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Window,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "..\.." @args
}

New-Alias i~ Edit-Home
function Edit-Home {
  param (
    [PathCompletions("~", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Window,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "~" @args
}

New-Alias ic Edit-Code
function Edit-Code {
  param (
    [PathCompletions("~\code", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Window,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "~\code" @args
}

New-Alias i\ Edit-Drive
New-Alias i/ Edit-Drive
function Edit-Drive {
  param (
    [PathCompletions("\", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Window,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "\" @args
}
