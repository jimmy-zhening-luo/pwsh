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
    [Alias('Run')]
    [string]$Script,
    [PathCompletions('~\code', 'Directory', $true)]
    [string]$Path
  )

  if (-not $Script) {
    throw 'No script name provided'
  }

  if ($Path.StartsWith(('-'))) {
    $Path, $args = '', (, $Path + $args)
  }

  $Prefix = Resolve-NodeProject @PSBoundParameters -ErrorAction Stop

  if ($Prefix) {
    $args = '--prefix', $Prefix + $args
  }

  & npm run $Script @args
}
