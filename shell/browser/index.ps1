. $PSScriptRoot\test.ps1

New-Alias open Open-Url
New-Alias o Open-Url
function Open-Url {
  [CmdletBinding(
    SupportsShouldProcess,
    DefaultParameterSetName = "Path"
  )]
  param(
    [Parameter(
      ParameterSetName = "Path",
      Position = 0
    )]
    [string]$Path = ".",
    [Parameter(
      ParameterSetName = "Uri",
      Position = 0,
      Mandatory
    )]
    [Uri]$Uri
  )

  switch ($PSCmdlet.ParameterSetName) {
    "Path" {
      $Splat = (Test-Path $Path) ? @{
        Path = Resolve-Path $Path
      } : @{
        Uri = $Path
      }
    }
    "Uri" {
      $Splat = @{
        Uri = $Uri
      }
    }
  }

  if (
    $PSCmdlet.ShouldProcess(
      $Splat,
      "chrome"
    )
  ) {
    Start-Process "C:\Program Files\Google\Chrome\Application\chrome.exe" ([array]$Splat.Values)[0]
  }
}
