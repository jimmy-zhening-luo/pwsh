
<#
.SYNOPSIS
Opens the 'Microsoft Store' app to the 'Updates & downloads' page.

.DESCRIPTION
This function invokes the URI 'ms-windows-store://downloadsandupdates' to open the 'Updates & downloads' page in the 'Microsoft Store' app.

.COMPONENT
WindowsSystem.App
#>
function Update-StoreApp {

  [CmdletBinding()]

  [OutputType([void])]

  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot launch Microsoft Store app during SSH session'
  }

  [hashtable]$Private:Store = @{
    FilePath = 'ms-windows-store://downloadsandupdates'
  }
  Start-Process @Store
}

$WINGET = "$HOME\AppData\Local\Microsoft\WindowsApps\winget.exe"

<#
.SYNOPSIS
Use WinGet to install a new package or upgrade an existing package.

.DESCRIPTION
This function is an alias for 'winget install', unless no arguments are provided, in which case it calls 'winget upgrade' with no arguments, listing apps with available upgrades.

.COMPONENT
WindowsSystem.App

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install
#>
function Add-WinGetApp {

  if ($args) {
    & $WINGET install @args
  }
  else {
    & $WINGET upgrade
  }

  if ($LASTEXITCODE -ne 0) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

<#
.SYNOPSIS
Use WinGet to check for a package upgrade or to upgrade a package.

.DESCRIPTION
This function is an alias for 'winget upgrade'.

.COMPONENT
WindowsSystem.App

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade
#>
function Update-WinGetApp {

  & $WINGET upgrade @args

  if ($LASTEXITCODE -ne 0) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

<#
.SYNOPSIS
Use WinGet to search WinGet repositories for a package.

.DESCRIPTION
This function is an alias for 'winget search', unless no arguments are provided, in which case it calls 'winget list' with no arguments, listing all available packages.

.COMPONENT
WindowsSystem.App

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/search
#>
function Find-WinGetApp {

  if ($args) {
    & $WINGET search @args
  }
  else {
    & $WINGET list
  }

  if ($LASTEXITCODE -ne 0) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

<#
.SYNOPSIS
Use WinGet to uninstall a package.

.DESCRIPTION
This function is an alias for 'winget uninstall'.

.COMPONENT
WindowsSystem.App

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall
#>
function Remove-WinGetApp {

  & $WINGET uninstall @args

  if ($LASTEXITCODE -ne 0) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

New-Alias gapx Appx\Get-AppxPackage
New-Alias remapx Appx\Remove-AppxPackage

New-Alias su Update-StoreApp
New-Alias wget $WINGET
New-Alias wga Add-WinGetApp
New-Alias wgu Update-WinGetApp
New-Alias wgf Find-WinGetApp
New-Alias wgr Remove-WinGetApp
