New-Alias gapx Appx\Get-AppxPackage
New-Alias remapx Appx\Remove-AppxPackage

New-Alias su Update-StoreApp
<#
.SYNOPSIS
Opens the 'Microsoft Store' app to the 'Updates & downloads' page.
.DESCRIPTION
This function invokes the URI 'ms-windows-store://downloadsandupdates' to open the 'Updates & downloads' page in the 'Microsoft Store' app.
#>
function Update-StoreApp {
  [OutputType([void])]
  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot launch Microsoft Store app during SSH session'
  }

  $Store = @{
    FilePath = 'ms-windows-store://downloadsandupdates'
  }
  Start-Process @Store
}

New-Alias wget winget.exe

New-Alias wga Add-WinGetApp
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
    & winget.exe install @args
  }
  else {
    & winget.exe upgrade
  }
}

New-Alias wgu Update-WinGetApp
<#
.SYNOPSIS
Use WinGet to check for a package upgrade or to upgrade a package.
.DESCRIPTION
This function is an alias for 'winget upgrade'.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade
#>
function Update-WinGetApp {
  & winget.exe upgrade @args
}

New-Alias wgf Find-WinGetApp
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
    & winget.exe search @args
  }
  else {
    & winget.exe list
  }
}

New-Alias wgr Remove-WinGetApp
<#
.SYNOPSIS
Use WinGet to uninstall a package.
.DESCRIPTION
This function is an alias for 'winget uninstall'.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall
#>
function Remove-WinGetApp {
  & winget.exe uninstall @args
}
