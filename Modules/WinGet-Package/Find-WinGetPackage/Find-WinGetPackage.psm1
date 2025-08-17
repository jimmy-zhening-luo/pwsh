New-Alias wgf Find-WinGetPackage
<#
.SYNOPSIS
Use WinGet to search WinGet repositories for a package.
.DESCRIPTION
This function is an alias for `winget search`, unless no arguments are provided, in which case it calls `winget list` with no arguments, listing all available packages.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/search
#>
function Find-WinGetPackage {
  if ($args.Length -eq 0) {
    winget list
  }
  else {
    winget search @args
  }
}
