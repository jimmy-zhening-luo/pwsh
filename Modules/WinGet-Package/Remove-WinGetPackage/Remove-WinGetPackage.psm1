New-Alias wgr Remove-WinGetPackage
<#
.SYNOPSIS
Use WinGet to uninstall a package.
.DESCRIPTION
This function is an alias for 'winget uninstall'.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/uninstall
#>
function Remove-WinGetPackage {
  [OutputType([string[]])]
  param()

  & winget uninstall @args
}
