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
  & cmd.exe /c @args
}

New-Alias wu Update-Windows
<#
.SYNOPSIS
Opens the 'Settings' app to the 'Windows Update' page.
.DESCRIPTION
This function invokes the URI 'ms-settings:windowsupdate' to open the 'Windows Update' page in the 'Settings' app.
#>
function Update-Windows {
  [CmdletBinding()]
  [OutputType([void])]
  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot open Settings app during SSH session'
  }

  [hashtable]$Private:WindowsUpdate = @{
    FilePath = 'ms-settings:windowsupdate'
  }
  Start-Process @WindowsUpdate
}

New-Alias path Edit-SystemPath
<#
.SYNOPSIS
Opens the 'Environment Variables' dialog as a standalone window.
.DESCRIPTION
This function invokes 'rundll32' on 'sysdm.cpl' ('System Properties' control panel) with the 'EditEnvironmentVariables' argument, which opens the 'Environment Variables' dialog directly.
#>
function Edit-SystemPath {
  [CmdletBinding()]
  [OutputType([void])]
  param(
    # Launch Environment Variables control panel as administrator to edit system variables
    [switch]$Administrator
  )

  if ($env:SSH_CLIENT) {
    throw 'Cannot present Control Panel during SSH session'
  }

  [hashtable]$Private:ControlPanel = @{
    FilePath     = 'rundll32'
    ArgumentList = @(
      'sysdm.cpl'
      'EditEnvironmentVariables'
    )
  }
  if ($Administrator) {
    $ControlPanel.Verb = 'RunAs'
  }
  Start-Process @ControlPanel
}

New-Alias sesv Set-Service
New-Alias remsv Remove-Service

New-Alias tkill Stop-Task
function Stop-Task {
  [CmdletBinding(
    DefaultParameterSetName = 'Name',
    SupportsShouldProcess
  )]
  [OutputType([void])]
  param(
    [Parameter(
      ParameterSetName = 'Name',
      Position = 0
    )]
    [string]$Name,
    [Parameter(
      ParameterSetName = 'Id',
      Mandatory,
      Position = 0
    )]
    [UInt32]$Id,
    [Parameter(
      ParameterSetName = 'Self',
      Mandatory
    )]
    [UInt32]$Self
  )

  [hashtable]$Private:Process = @{
    Force = $True
  }
  switch ($PSCmdlet.ParameterSetName) {
    Self {
      $Process.Name = 'windowsterminal'
    }
    Id {
      $Process.Id = $Id
      break
    }
    default {
      if ($Name) {
        if ($Name -match [regex]'^(?>\d{1,10})$' -and $Name -as [UInt32]) {
          $Process.Id = [UInt32]$Name
        }
        else {
          $Process.Name = $Name
        }
      }
      else {
        $Process.Name = 'explorer'
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
    Stop-Process @Process
  }
}
