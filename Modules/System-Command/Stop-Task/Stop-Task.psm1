New-Alias tkill Stop-Task
New-Alias tkillx Stop-Task
function Stop-Task {
  [OutputType([void], [System.Diagnostics.Process[]])]
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

  $Parameters = @{
    Force = $true
  }

  if ($Id) {
    $Parameters.Id = $Id -as [uint32]
  }
  else {
    if ($Name -match '^\s*\d{1,10}\s*$' -and $Name -as [uint32]) {
      $Parameters.Id = $Name -as [uint32]
    }
    else {
      $Parameters.Name = $Name
    }
  }

  if (
    $PSCmdlet.ShouldProcess(
      "Name:$($Parameters.Name) or Id:$($Parameters.Id)",
      'Stop-Process'
    )
  ) {
    Stop-Process @Parameters
  }
}
