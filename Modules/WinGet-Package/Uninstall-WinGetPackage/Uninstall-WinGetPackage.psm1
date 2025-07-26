New-Alias wgr Uninstall-WinGetPackage
<#
.SYNOPSIS
Use WinGet to uninstall a package.

.DESCRIPTION
This function is an alias for `winget uninstall`.

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall
#>
function Uninstall-WinGetPackage {
  winget uninstall @args
}
