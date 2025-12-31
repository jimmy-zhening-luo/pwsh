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
  [Alias('tkill')]
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
    [switch]$Self
  )

  process {
    switch ($PSCmdlet.ParameterSetName) {
      Id {
        foreach ($i in $Id) {
          (
            Get-Process -Id $i
          ).Kill($True)
        }
      }
      Name {
        switch -Regex ($Name) {
          '^$' { continue }
          '^(?>\d{1,10})$' {
            if (
              $PSCmdlet.ShouldProcess(
                "Process ID: $PSItem",
                '(Get-Process).Kill($True)'
              )
            ) {
              (
                Get-Process -Id ([uint]$PSItem)
              ).Kill($True)
            }

            continue
          }
          default {
            if (
              $PSCmdlet.ShouldProcess(
                "Process Name: $PSItem",
                '(Get-Process).Kill($True)'
              )
            ) {
              (
                Get-Process -Name $PSItem
              ).Kill($True)
            }
          }
        }
      }
    }
  }

  end {
    switch ($PSCmdlet.ParameterSetName) {
      Self {
        if ($Self) {
          if (
            $PSCmdlet.ShouldProcess(
              'Process Name: -Self [=> WindowsTerminal]',
              '(Get-Process).Kill($True)'
            )
          ) {
            (
              Get-Process -Name WindowsTerminal
            ).Kill($True)
          }
        }
        return
      }
      Name {
        if (-not $Name) {
          if (
            $PSCmdlet.ShouldProcess(
              'Process Name: [=> explorer]',
              '(Get-Process).Kill($True)'
            )
          ) {
            (
              Get-Process -Name explorer
            ).Kill($True)
          }
        }
      }
    }
  }
}

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
  [Alias('wu')]
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
  [Alias('path')]
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

New-Alias restart Restart-Computer
New-Alias sesv Set-Service
New-Alias remsv Remove-Service
