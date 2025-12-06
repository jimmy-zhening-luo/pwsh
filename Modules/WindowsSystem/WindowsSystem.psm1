New-Alias restart Restart-Computer

<#
.SYNOPSIS
Runs a command in the Windows Command Prompt, 'cmd.exe'.
.DESCRIPTION
Starts a new instance of the command interpreter, 'cmd.exe' to carry out the supplied command (along with any flags) before exiting the command processor.
.LINK
https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
#>
function Invoke-CommandPrompt {
  & cmd /c $args
}

New-Alias wu WindowsSystem\Update-Windows
<#
.SYNOPSIS
Opens the 'Settings' app to the 'Windows Update' page.
.DESCRIPTION
This function invokes the URI 'ms-settings:windowsupdate' to open the 'Windows Update' page in the 'Settings' app.
#>
function Update-Windows {
  [OutputType([void])]
  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot open Settings app during SSH session'
  }

  $WindowsUpdate = @{
    FilePath = 'ms-settings:windowsupdate'
  }
  [void](Start-Process @WindowsUpdate)
}

New-Alias path WindowsSystem\Edit-Path
<#
.SYNOPSIS
Opens the 'Environment Variables' dialog as a standalone window.
.DESCRIPTION
This function invokes 'rundll32' on 'sysdm.cpl' ('System Properties' control panel) with the 'EditEnvironmentVariables' argument, which opens the 'Environment Variables' dialog directly.
#>
function Edit-Path {
  [OutputType([void])]
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

New-Alias sesv Set-Service
New-Alias remsv Remove-Service

New-Alias tkill WindowsSystem\Stop-Task
function Stop-Task {
  [OutputType([void])]
  [CmdletBinding(
    SupportsShouldProcess,
    DefaultParameterSetName = 'Name'
  )]
  param(
    [Parameter(
      ParameterSetName = 'Name',
      Position = 0
    )]
    [string]$Name = 'explorer',
    [Parameter(
      ParameterSetName = 'Id',
      Mandatory,
      Position = 0
    )]
    [UInt32]$Id
  )

  $Process = @{
    Force = $True
  }
  switch ($PSCmdlet.ParameterSetName) {
    Id {
      $Process.Id = $Id
      break
    }
    default {
      if ($Name -match '^(?>\d{1,10})$' -and $Name -as [UInt32]) {
        $Process.Id = [UInt32]$Name
      }
      else {
        $Process.Name = $Name
      }
    }
  }

  if (-not $Process.Id -and -not $Process.Name) {
    throw 'Must specify a valid PID or process name'
  }

  if (
    $PSCmdlet.ShouldProcess(
      $Process.Id ? "Process ID: $($Process.Id)" : "Process Name: $($Process.Name)",
      'Stop-Process'
    )
  ) {
    [void](Stop-Process @Process)
  }
}
