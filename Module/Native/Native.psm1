@(
  @{
    Name = 'clb'
    Value = 'Clear-RecycleBin'
  }
  @{
    Name = 'k'
    Value = 'Get-Member'
  }
  @{
    Name = 'key'
    Value = 'Get-Member'
  }
  @{
    Name = 'psk'
    Value = 'Get-PSReadLineKeyHandler'
  }
  @{
    Name = 'verb'
    Value = 'Get-VerbList'
  }
  @{
    Name = 'ct'
    Value = 'Measure-Object'
  }
  @{
    Name = 'count'
    Value = 'Measure-Object'
  }
  @{
    Name = 'touch'
    Value = 'New-Item'
  }
  @{
    Name = 'remsv'
    Value = 'Remove-Service'
  }
  @{
    Name = 'restart'
    Value = 'Restart-Computer'
  }
  @{
    Name = '^'
    Value = 'Select-Object'
  }
  @{
    Name = 's'
    Value = 'Select-Object'
  }
  @{
    Name = 'sesv'
    Value = 'Set-Service'
  }
  @{
    Name = 'z'
    Value = 'Sort-Object'
  }
  @{
    Name = 'upman'
    Value = 'Update-Help'
  }
  @{
    Name = 'wg'
    Value = "$env:LOCALAPPDATA\Microsoft\WindowsApps\winget.exe"
  }
) | New-Alias

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
Use WinGet to search for a package.

.DESCRIPTION
This command is an alias for 'winget search'. When invoked with no arguments, it instead aliases 'winget list' to list all installed packages.

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
This command is an alias for 'winget uninstall'.

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
