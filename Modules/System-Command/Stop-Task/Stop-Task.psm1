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
    [string]$Name = "explorer",
    [Parameter(
      ParameterSetName = "Id",
      Position = 0,
      Mandatory
    )]
    [int]$Id
  )

  switch ($PSCmdlet.ParameterSetName) {
    "Name" {
      $Params = @{
        Name = $Name
      }
    }
    "Id" {
      $Params = @{
        Id = $Id
      }
    }
  }

  if (
    $PSCmdlet.ShouldProcess(
      "Name:$Name or Id:$Id",
      "Stop-Process"
    )
  ) {
    Stop-Process -Force @Params
  }
}
