New-Alias npo Compare-Package
<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages.
.DESCRIPTION
This function is an alias for `npm outdated` `[--prefix=$Path]`.
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-Package {
  param([System.String]$Path)

  if ($Path) {
    if (Test-Path $Path -PathType Container) {
      $AbsolutePath = (Resolve-Path $Path).Path

      npm outdated --prefix=$AbsolutePath
    }
    else {
      throw "Path '$Path' is not a directory."
    }
  }
  else {
    npm outdated
  }
}
