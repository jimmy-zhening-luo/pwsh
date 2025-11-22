New-Alias wga Add-WinGetPackage
<#
.SYNOPSIS
Use WinGet to install a new package or upgrade an existing package.
.DESCRIPTION
This function is an alias for 'winget install', unless no arguments are provided, in which case it calls 'winget upgrade' with no arguments, listing apps with available upgrades.
.LINK
https://learn.microsoft.com/en-us/windows/package-manager/winget/install
#>
function Add-WinGetPackage {
  param(
    [Parameter(
      ValueFromRemainingArguments
    )]
    [string[]]$Arguments
  )

  if ($Arguments -or $args) {
    & winget install @Arguments @args
  }
  else {
    & winget upgrade
  }
}
