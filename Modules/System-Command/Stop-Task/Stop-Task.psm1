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
    [uint32]$Id
  )

  if (
    $PSCmdlet.ShouldProcess(
      "Name:$Name or Id:$Id",
      'Stop-Process'
    )
  ) {
    switch ($PSCmdlet.ParameterSetName) {
      'Name' {
        Stop-Process -Name $Name -Force
      }
      'Id' {
        Stop-Process -Id $Id -Force
      }
    }
  }
}
