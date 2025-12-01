New-Alias path WindowsSystem\Edit-Path
<#
.SYNOPSIS
Opens the 'Environment Variables' dialog as a standalone window.
.DESCRIPTION
This function invokes 'rundll32' on 'sysdm.cpl' ('System Properties' control panel) with the 'EditEnvironmentVariables' argument, which opens the 'Environment Variables' dialog directly.
#>
function Edit-Path {
  param(
    # Launch Environment Variables control panel as administrator to edit system variables
    [switch]$Administrator
  )

  if ($env:SSH_CLIENT) {
    throw 'Cannot present Control Panel during SSH session'
  }

  $ControlPanel = @{
    FilePath     = 'rundll32'
    ArgumentList = @(
      'sysdm.cpl'
      'EditEnvironmentVariables'
    )
  }

  if ($Administrator) {
    $ControlPanel.Verb = 'RunAs'
  }

  [void](Start-Process @ControlPanel)
}
