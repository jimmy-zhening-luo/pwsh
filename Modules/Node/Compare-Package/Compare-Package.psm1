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

  $Local:args = $args
  $Prefix = Resolve-NodeProject @PSBoundParameters -ErrorAction Stop

  if ($Prefix) {
    $Local:args = '--prefix', $Prefix + $Local:args
  }

  & npm outdated @Local:args
}
