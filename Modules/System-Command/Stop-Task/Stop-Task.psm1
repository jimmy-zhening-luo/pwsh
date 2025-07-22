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
      if (
        $PSCmdlet.ShouldProcess(
          $Name,
          "Stop-Process -Name"
        )
      ) {
        Stop-Process -Force -Name $Name
      }
    }
    "Id" {
      if (
        $PSCmdlet.ShouldProcess(
          $Id,
          "Stop-Process -Id"
        )
      ) {
        Stop-Process -Force -Id $Id
      }
    }
  }


}
