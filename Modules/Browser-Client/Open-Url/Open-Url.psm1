New-Alias open Open-Url
New-Alias o Open-Url
function Open-Url {
  [CmdletBinding(
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

  $Target = ($PSCmdlet.ParameterSetName -eq "Uri") ? ($Uri) : (
    (Test-Path $Path) ? (Resolve-Path $Path) : ($Path)
  )

  Start-Process "C:\Program Files\Google\Chrome\Application\chrome.exe" $Target
}
