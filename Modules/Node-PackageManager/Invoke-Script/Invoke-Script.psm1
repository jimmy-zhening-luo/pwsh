New-Alias nr Invoke-Script
<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a package's 'package.json'.
.DESCRIPTION
This function is an alias for 'npm run [script] [--args]'.
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Invoke-Script {
  param(
    [string]$Script,
    [string]$Path
  )

  if (-not $Script) {
    throw 'No script name provided'
  }

  $NpmOption = ''

  if ($Path.StartsWith(('-'))) {
    $NpmOption = $Path
    $Path = $null
  }

  if ($Path) {
    $AbsolutePath = Resolve-NodeProject $Path

    if ($AbsolutePath) {
      if ($NpmOption) {
        & npm run $Script --prefix $AbsolutePath $NpmOption @args
      }
      else {
        & npm run $Script --prefix $AbsolutePath @args
      }
    }
    else {
      throw "Path '$Path' is not a Node project directory."
    }
  }
  else {
    if ($NpmOption) {
      & npm run $Script $NpmOption @args
    }
    else {
      & npm run $Script @args
    }
  }
}
