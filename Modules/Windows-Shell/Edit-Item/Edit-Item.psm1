New-Alias i Edit-Item
<#
.SYNOPSIS
Edit a file in Visual Studio Code.
.DESCRIPTION
This function is an alias for the Visual Studio Code command line interface, `code.cmd`.
.LINK
https://code.visualstudio.com/docs/configure/command-line
#>
function Edit-Item {
  [OutputType([void])]
  param(
    [Parameter(Position = 0)]
    [PathCompletions(".", "")]
    [string]$Path,
    [Parameter(Position = 1)]
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow,
    [Parameter()]
    [string]$RootPath
  )

  if ($RootPath) {
    if (-not (Test-Path -Path $RootPath -PathType Container)) {
      $CommandArguments += $RootPath
      $RootPath = ''
    }
  }

  $FullPath = $RootPath ? (Join-Path $RootPath $Path) : $Path
  $CommandArguments = @()

  if ($Path) {
    if (Test-Path -Path $FullPath) {
      $CommandArguments = , (Resolve-Path -Path $FullPath) + $CommandArguments
    }
    else {
      if (-not $Path.StartsWith("-")) {
        throw "Path '$FullPath' does not exist."
      }

      $FullPath = ($RootPath ? (Resolve-Path -Path $RootPath -) : ".")
      $CommandArguments = $FullPath, $Path + $CommandArguments
    }
  }
  else {
    if ($FullPath) {
      $CommandArguments = , $FullPath + $CommandArguments
    }
  }

  if ($env:SSH_CLIENT) {
    if (Test-Path -Path $FullPath -PathType Container) {
      Set-Location -Path $FullPath
    }
    else {
      Read-Item -Path $FullPath
    }
  }
  else {
    if ($ProfileName) {
      if (-not $ProfileName.StartsWith("-")) {
        $Force = $true

        $CommandArguments += "--profile"
      }

      $CommandArguments += $ProfileName
    }

    if ($Force) {
      $CommandArguments += "--new-window"
    }
    elseif ($ReuseWindow) {
      $CommandArguments += "--reuse-window"
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
<#
.FORWARDHELPTARGETNAME Edit-Item
.FORWARDHELPCATEGORY Function
#>
function Edit-Sibling {
  param (
    [PathCompletions("..", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath ".." @args
}

New-Alias i.. Edit-Relative
<#
.FORWARDHELPTARGETNAME Edit-Item
.FORWARDHELPCATEGORY Function
#>
function Edit-Relative {
  param (
    [PathCompletions("..\..", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "..\.." @args
}

New-Alias i~ Edit-Home
<#
.FORWARDHELPTARGETNAME Edit-Item
.FORWARDHELPCATEGORY Function
#>
function Edit-Home {
  param (
    [PathCompletions("~", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "~" @args
}

New-Alias ic Edit-Code
<#
.FORWARDHELPTARGETNAME Edit-Item
.FORWARDHELPCATEGORY Function
#>
function Edit-Code {
  param (
    [PathCompletions("~\code", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath ~\code @args
}

New-Alias i\ Edit-Drive
New-Alias i/ Edit-Drive
<#
.FORWARDHELPTARGETNAME Edit-Item
.FORWARDHELPCATEGORY Function
#>
function Edit-Drive {
  param (
    [PathCompletions("\", "")]
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  Edit-Item @PSBoundParameters -RootPath "\" @args
}
