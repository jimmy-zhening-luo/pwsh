<#
.SYNOPSIS
Use WinGet to install a new package, upgrade an existing package, or list all packages with available updates.

.DESCRIPTION
This command is an alias for 'winget install' and can be used to install or upgrade a package. When invoked with no arguments, it instead aliases 'winget upgrade' to list all packages with available updates.

.COMPONENT
Native

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/upgrade
#>
function Add-WinGetApp {

  [Alias('wga')]
  param()

  if ($args) {
    & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe install @args
  }
  else {
    & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe upgrade
  }

  if ($LASTEXITCODE -notin 0, 1) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

<#
.SYNOPSIS
Use WinGet to search WinGet repositories for a package.

.DESCRIPTION
This function is an alias for 'winget search', unless no arguments are provided, in which case it calls 'winget list' with no arguments, listing all available packages.

.COMPONENT
Native

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/search

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/list
#>
function Find-WinGetApp {

  [Alias('wgf')]
  param()

  if ($args) {
    & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe search @args
  }
  else {
    & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe list
  }

  if ($LASTEXITCODE -notin 0, 1) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

<#
.SYNOPSIS
Use WinGet to uninstall a package.

.DESCRIPTION
This function is an alias for 'winget uninstall'.

.COMPONENT
Native

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall
#>
function Remove-WinGetApp {

  [Alias('wgr')]
  param()

  & $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe uninstall @args

  if ($LASTEXITCODE -notin 0, 1) {
    throw "winget.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

New-Alias ^ Select-Object
New-Alias s Select-Object
New-Alias sel Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias keys Get-Member
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias tab Format-Table
New-Alias table Format-Table
New-Alias format Format-Table

New-Alias split Split-Path
New-Alias hash Get-FileHash
New-Alias touch New-Item

New-Alias upman Update-Help
New-Alias psk Get-PSReadLineKeyHandler

New-Alias restart Restart-Computer
New-Alias clb Clear-RecycleBin
New-Alias sesv Set-Service
New-Alias remsv Remove-Service
New-Alias gapx Get-AppxPackage
New-Alias remapx Remove-AppxPackage
New-Alias wg $env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe
