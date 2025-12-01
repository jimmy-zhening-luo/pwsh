New-Alias wgf WindowsSystem\Find-WinGetApp
<#
.SYNOPSIS
Use WinGet to search WinGet repositories for a package.
.DESCRIPTION
This function is an alias for 'winget search', unless no arguments are provided, in which case it calls 'winget list' with no arguments, listing all available packages.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/search
#>
function Find-WinGetApp {
  if ($args) {
    & winget search @args
  }
  else {
    & winget list
  }
}
