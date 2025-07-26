New-Alias wgu Sync-WinGetPackage
<#
.SYNOPSIS
Use WinGet to check for a package upgrade or to upgrade a package.

.DESCRIPTION
This function is an alias for `winget upgrade`.

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade
#>
function Sync-WinGetPackage {
  winget upgrade @args
}
