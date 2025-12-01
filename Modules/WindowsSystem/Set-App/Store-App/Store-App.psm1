New-Alias gapx Get-AppxPackage
New-Alias remapx Remove-AppxPackage

New-Alias su WindowsSystem\Update-StoreApp
<#
.SYNOPSIS
Opens the 'Microsoft Store' app to the 'Updates & downloads' page.
.DESCRIPTION
This function invokes the URI 'ms-windows-store://downloadsandupdates' to open the 'Updates & downloads' page in the 'Microsoft Store' app.
#>
function Update-StoreApp {
  if ($env:SSH_CLIENT) {
    throw 'Cannot launch Microsoft Store app during SSH session'
  }

  [void](Start-Process -FilePath ms-windows-store://downloadsandupdates)
}
