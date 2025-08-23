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
  param([System.String]$Path)

  if ($env:SSH_CLIENT) {
    throw "Cannot launch Visual Studio Code from SSH client."
  }
  else {
    if ($Path) {
      if (Test-Path $Path) {
        code.cmd $Path @args
      }
      else {
        if ($Path.StartsWith("-")) {
          code.cmd $PWD $Path @args
        }
        else {
          throw "File '$Path' does not exist."
        }
      }
    }
    else {
      code.cmd $PWD
    }
  }
}
