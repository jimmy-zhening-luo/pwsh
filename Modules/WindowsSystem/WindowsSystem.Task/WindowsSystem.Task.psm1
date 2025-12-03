New-Alias sesv Set-Service
New-Alias remsv Remove-Service

New-Alias tkill WindowsSystem\Stop-Task
function Stop-Task {
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
    'Id' {
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
    Stop-Process @Process
  }
}
