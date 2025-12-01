New-Alias wgu WindowsSystem\Update-WinGetApp
<#
.SYNOPSIS
Use WinGet to check for a package upgrade or to upgrade a package.
.DESCRIPTION
This function is an alias for 'winget upgrade'.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade
#>
function Update-WinGetApp {
  & winget upgrade @args
}
