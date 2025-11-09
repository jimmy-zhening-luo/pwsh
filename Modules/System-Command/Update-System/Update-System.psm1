New-Alias wu Update-Windows
<#
.SYNOPSIS
Opens the `Settings` app to the `Windows Update` page.
.DESCRIPTION
This function invokes the URI `ms-settings:windowsupdate` to open the `Windows Update` page in the `Settings` app.
#>
function Update-Windows {
  [OutputType([void])]
  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot open Settings app during SSH session'
  }

  Start-Process ms-settings:windowsupdate
}

New-Alias su Update-StoreApp
<#
.SYNOPSIS
Opens the `Microsoft Store` app to the `Updates & downloads` page.
.DESCRIPTION
This function invokes the URI `ms-windows-store://downloadsandupdates` to open the `Updates & downloads` page in the `Microsoft Store` app.
#>
function Update-StoreApp {
  [OutputType([void])]
  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot launch Microsoft Store app during SSH session'
  }

  Start-Process ms-windows-store://downloadsandupdates
}
