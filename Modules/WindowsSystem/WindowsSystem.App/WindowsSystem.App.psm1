Microsoft.PowerShell.Utility\New-Alias gapx Appx\Get-AppxPackage
Microsoft.PowerShell.Utility\New-Alias remapx Appx\Remove-AppxPackage

Microsoft.PowerShell.Utility\New-Alias su WindowsSystem\Update-StoreApp
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
  [void](Microsoft.PowerShell.Management\Start-Process @Store)
}

Microsoft.PowerShell.Utility\New-Alias wget winget.exe

Microsoft.PowerShell.Utility\New-Alias wga WindowsSystem\Add-WinGetApp
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

Microsoft.PowerShell.Utility\New-Alias wgu WindowsSystem\Update-WinGetApp
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

Microsoft.PowerShell.Utility\New-Alias wgf WindowsSystem\Find-WinGetApp
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

Microsoft.PowerShell.Utility\New-Alias wgr WindowsSystem\Remove-WinGetApp
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
