New-Alias wu WindowsSystem\Update-Windows
<#
.SYNOPSIS
Opens the 'Settings' app to the 'Windows Update' page.
.DESCRIPTION
This function invokes the URI 'ms-settings:windowsupdate' to open the 'Windows Update' page in the 'Settings' app.
#>
function Update-Windows {
  if ($env:SSH_CLIENT) {
    throw 'Cannot open Settings app during SSH session'
  }

  [void](Start-Process -FilePath ms-settings:windowsupdate)
}
