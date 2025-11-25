New-Alias npc Clear-PackageCache
<#
.SYNOPSIS
Use Node Package Manager (npm) to clear package cache.
.DESCRIPTION
This function is an alias for 'npm cache clean --force'.
.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-PackageCache {
  & npm cache clean --force @args
}
