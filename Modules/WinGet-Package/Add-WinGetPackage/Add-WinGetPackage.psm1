New-Alias wga Add-WinGetPackage
<#
.SYNOPSIS
Use WinGet to install a new package or upgrade an existing package.

.DESCRIPTION
This function is an alias for `winget install`.

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install
#>
function Add-WinGetPackage {
  winget install @args
}
