New-Alias path Edit-Path
<#
.SYNOPSIS
Opens the `Environment Variables` dialog as a standalone window.
.DESCRIPTION
This function invokes `rundll32` on `sysdm.cpl` (`System Properties` control panel) with the `EditEnvironmentVariables` argument, which opens the `Environment Variables` dialog directly.
#>
function Edit-Path {
  if ($env:SSH_CLIENT) {
    Write-Warning 'Cannot launch Control Panel dialog during SSH session'
    return
  }

  rundll32 sysdm.cpl, EditEnvironmentVariables
}
