
<#
.SYNOPSIS
Runs a command in the Windows Command Prompt, 'cmd.exe'.

.DESCRIPTION
Starts a new instance of the command interpreter, 'cmd.exe' to carry out the supplied command (along with any flags) before exiting the command processor.

.COMPONENT
WindowsSystem

.LINK
https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
#>
function Invoke-CommandPrompt {

  & $env:ComSpec /c @args

  if ($LASTEXITCODE -notin 0, 1) {
    throw "cmd.exe error, execution stopped with exit code: $LASTEXITCODE"
  }
}

<#
.SYNOPSIS
Opens the 'Settings' app to the 'Windows Update' page.

.DESCRIPTION
This function invokes the URI 'ms-settings:windowsupdate' to open the 'Windows Update' page in the 'Settings' app.

.COMPONENT
WindowsSystem
#>
function Update-Windows {
  [CmdletBinding()]
  [OutputType([void])]
  param()

  if ($env:SSH_CLIENT) {
    throw 'Cannot open Settings app during SSH session'
  }

  Start-Process -FilePath ms-settings:windowsupdate
}

<#
.SYNOPSIS
Opens the 'Environment Variables' dialog as a standalone window.

.DESCRIPTION
This function invokes 'rundll32' on 'sysdm.cpl' ('System Properties' control panel) with the 'EditEnvironmentVariables' argument, which opens the 'Environment Variables' dialog directly.

.COMPONENT
WindowsSystem
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

  $ControlPanel = @{
    FilePath     = "$env:SystemRoot\System32\rundll32.exe"
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

<#
.SYNOPSIS
Stops one or more running processes by name or ID.

.DESCRIPTION
Stops one or more running processes by name or ID. If no name or ID is provided, the function defaults to stopping 'explorer.exe'.

If the Self switch is used, the function stops the Windows Terminal process itself and ignores other parameters.

.COMPONENT
WindowsSystem
#>
function Stop-Task {
  [CmdletBinding(
    DefaultParameterSetName = 'Name',
    SupportsShouldProcess,
    ConfirmImpact = 'Medium'
  )]
  [OutputType([void])]
  param(

    [Parameter(
      ParameterSetName = 'Name',
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [SupportsWildcards()]
    [Alias('ProcessName')]
    # The name(s) of the process to stop.
    [string[]]$Name,

    [Parameter(
      ParameterSetName = 'Id',
      Mandatory,
      Position = 0,
      ValueFromPipelineByPropertyName
    )]
    # The process ID(s) of the process to stop.
    [uint[]]$Id,

    [Parameter(
      ParameterSetName = 'Self',
      Mandatory
    )]
    [switch]$Self,

    [Parameter(DontShow)][switch]$z
  )

  process {
    switch ($PSCmdlet.ParameterSetName) {
      Id {
        foreach ($ProcessId in $Id) {
          Stop-Process -Id $ProcessId -Force
        }
      }
      Name {
        foreach ($ProcessHandle in $Name) {
          if ($ProcessHandle) {
            $Process = @{
              Force = $True
            }
            if ($ProcessHandle -match [regex]'^(?>\d{1,10})$' -and $ProcessHandle -as [uint]) {
              $Process.Id = [uint]$ProcessHandle
            }
            else {
              $Process.Name = $ProcessHandle
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
        }
      }
    }
  }

  end {
    switch ($PSCmdlet.ParameterSetName) {
      Name {
        if (-not $Name) {
          if (
            $PSCmdlet.ShouldProcess(
              'Process Name: explorer',
              'Stop-Process (default => explorer)'
            )
          ) {
            Stop-Process -Name explorer -Force
          }
        }
      }
      Self {
        if ($Self) {
          if (
            $PSCmdlet.ShouldProcess(
              'Process Name: WindowsTerminal',
              'Stop-Process (-Self => WindowsTerminal)'
            )
          ) {
            Stop-Process -Name WindowsTerminal -Force
          }
        }
      }
    }
  }
}

New-Alias restart Restart-Computer

New-Alias wu Update-Windows
New-Alias path Edit-SystemPath
New-Alias sesv Set-Service
New-Alias remsv Remove-Service
New-Alias tkill Stop-Task
