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
    [string]$Path,
    [Alias("pn")]
    [string]$ProfileName,
    [Alias("nw")]
    [switch]$NewWindow,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  if ($env:SSH_CLIENT) {
    Write-Warning 'Cannot launch Visual Studio Code during SSH session'
    return
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
