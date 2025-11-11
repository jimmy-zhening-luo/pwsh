New-Alias npo Compare-Package
<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages.
.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $Path]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-Package {
  param(
    [PathCompletions('~\code', 'Directory', $true)]
    [string]$Path
  )

  if ($Path) {
    $AbsolutePath = Resolve-NodeProject $Path

    if ($AbsolutePath) {
      & npm outdated --prefix $AbsolutePath
    }
    else {
      throw "Path '$Path' is not a Node project directory."
    }
  }
  else {
    & npm outdated
  }
}
