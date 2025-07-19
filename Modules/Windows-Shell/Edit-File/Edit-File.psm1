New-Alias -Name edit -Value Edit-File
New-Alias -Name i -Value Edit-File
<#
.SYNOPSIS
Edit a file in Visual Studio Code.

.DESCRIPTION
This function is an alias for the Visual Studio Code command line interface, `code.cmd`.

.LINK
https://code.visualstudio.com/docs/configure/command-line
#>
function Edit-File {
  param([string]$Path)

  if ($Path) {
    if (Test-Path $Path) {
      code.cmd $Path @args
    }
    else {
      if ($Path.StartsWith("-")) {
        code.cmd . $Path @args
      }
      else {
        throw "File '$Path' does not exist."
      }
    }
  }
  else {
    code.cmd .
  }
}
