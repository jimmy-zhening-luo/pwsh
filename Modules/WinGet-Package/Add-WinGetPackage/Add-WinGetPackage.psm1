New-Alias wga Add-WinGetPackage
<#
.SYNOPSIS
Use WinGet to install a new package or upgrade an existing package.
.DESCRIPTION
This function is an alias for `winget install`, unless no arguments are provided, in which case it calls `winget upgrade` with no arguments (lists apps with upgrades available).
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install
#>
function Add-WinGetPackage {
  if ($args.Length -eq 0) {
    winget upgrade
  }
  else {
    winget install @args
  }
}
