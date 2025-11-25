New-Alias tkill Stop-Task
New-Alias tkillx Stop-Task
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
    [string]$Id
  )

  $Task = @{
    Force = $true
  }

  if ($Id) {
    $Task.Id = $Id -as [uint32]
  }
  else {
    if ($Name -match '^\s*\d{1,10}\s*$' -and $Name -as [uint32]) {
      $Task.Id = $Name -as [uint32]
    }
    else {
      $Task.Name = $Name
    }
  }

  if (
    $PSCmdlet.ShouldProcess(
      "Name:$($Task.Name) or Id:$($Task.Id)",
      'Stop-Process'
    )
  ) {
    Stop-Process @Task
  }
}
