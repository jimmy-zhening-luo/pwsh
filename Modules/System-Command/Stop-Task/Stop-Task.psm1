New-Alias -Name tkill -Value Stop-Task -Option ReadOnly
New-Alias -Name tkillx -Value Stop-Task -Option ReadOnly
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
      $Splat = @{
        Name = $Name
      }
    }
    "Id" {
      $Splat = @{
        Id = $Id
      }
    }
  }

  $Splat.Add(
    "Force",
    $true
  )

  if (
    $PSCmdlet.ShouldProcess(
      $Splat,
      "Stop-Process"
    )
  ) {
    Stop-Process @Splat
  }
}
