New-Alias tkill Stop-Task
New-Alias tkillx Stop-Task
function Stop-Task {
  [CmdletBinding(
    SupportsShouldProcess,
    DefaultParameterSetName = "Name"
  )]
  param(
    [Parameter(
      ParameterSetName = "Name",
      Position = 0
    )]
    [System.String]$Name = "explorer",
    [Parameter(
      ParameterSetName = "Id",
      Position = 0,
      Mandatory
    )]
    [System.UInt32]$Id
  )

  if (
    $PSCmdlet.ShouldProcess(
      "Name:$Name or Id:$Id",
      "Stop-Process"
    )
  ) {
    switch ($PSCmdlet.ParameterSetName) {
      "Name" {
        Stop-Process -Force -Name $Name
      }
      "Id" {
        Stop-Process -Force -Id $Id
      }
    }
  }
}
