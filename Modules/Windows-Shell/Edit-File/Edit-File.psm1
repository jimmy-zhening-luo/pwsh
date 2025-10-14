New-Alias i Edit-File
<#
.SYNOPSIS
Edit a file in Visual Studio Code.
.DESCRIPTION
This function is an alias for the Visual Studio Code command line interface, `code.cmd`.
.LINK
https://code.visualstudio.com/docs/configure/command-line
#>
function Edit-File {
  param(
    [System.String]$Path,
    [Alias("pn")]
    [System.String]$ProfileName,
    [Alias("nw")]
    [switch]$NewWindow,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  if ($env:SSH_CLIENT) {
    throw "Cannot launch Visual Studio Code from SSH client."
  }

  $CodeArguments = @()

  if ($Path) {
    if (-not (Test-Path $Path)) {
      if (-not $Path.StartsWith("-")) {
        throw "Path '$Path' does not exist."
      }

      $CodeArguments += $PWD.Path
    }

    $CodeArguments += $Path
  }

  if ($ProfileName) {
    if (-not $ProfileName.StartsWith("-")) {
      $NewWindow = $true

      $CodeArguments += "--profile"
    }

    $CodeArguments += $ProfileName
  }

  if ($NewWindow) {
    $CodeArguments += "--new-window"
  }
  elseif ($ReuseWindow) {
    $CodeArguments += "--reuse-window"
  }

  if ($CodeArguments) {
    code.cmd $CodeArguments @args
  }
  else {
    code.cmd @args
  }

  return
}
