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
  [OutputType([void])]
  param(
    [string]$Path,
    [Alias("Name", "pn")]
    [string]$ProfileName,
    [Alias("Window")]
    [switch]$Force,
    [Alias("rw")]
    [switch]$ReuseWindow
  )

  if (-not $env:SSH_CLIENT) {
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
      [void](code.cmd $CodeArguments @args)
    }
    else {
      [void](code.cmd @args)
    }
  }
}
