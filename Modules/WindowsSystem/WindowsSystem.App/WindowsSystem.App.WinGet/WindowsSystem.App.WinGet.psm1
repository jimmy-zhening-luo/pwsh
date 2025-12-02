New-Alias wget winget

New-Alias wga WindowsSystem\Add-WinGetApp
<#
.SYNOPSIS
Use WinGet to install a new package or upgrade an existing package.
.DESCRIPTION
This function is an alias for 'winget install', unless no arguments are provided, in which case it calls 'winget upgrade' with no arguments, listing apps with available upgrades.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install
#>
function Add-WinGetApp {
  if ($args) {
    & winget install @args
  }
  else {
    & winget upgrade
  }
}

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

New-Alias wgr WindowsSystem\Remove-WinGetApp
<#
.SYNOPSIS
Use WinGet to uninstall a package.
.DESCRIPTION
This function is an alias for 'winget uninstall'.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall
#>
function Remove-WinGetApp {
  & winget uninstall @args
}
