New-Alias -Name wga -Value Add-WinGetPackage -Option ReadOnly
<#
.SYNOPSIS
Add a package using WinGet.

.DESCRIPTION
This function is an alias for `winget install` (alias: `winget add`).

.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install
#>
function Add-WinGetPackage {
  winget add @args
}
