New-Alias wgf Find-WinGetPackage
<#
.SYNOPSIS
Use WinGet to search WinGet repositories for a package.

.DESCRIPTION
This function is an alias for `winget search`.

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/search
#>
function Find-WinGetPackage {
  winget search @args
}
