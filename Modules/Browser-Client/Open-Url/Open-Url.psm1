New-Alias open Open-Url
New-Alias o Open-Url
function Open-Url {
  [OutputType([void])]
  [CmdletBinding(DefaultParameterSetName = "Path")]
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

  if (-not $env:SSH_CLIENT) {
    [void](
      Start-Process -FilePath "C:\Program Files\Google\Chrome\Application\chrome.exe" -ArgumentList (
        (
          $PSCmdlet.ParameterSetName -eq "Uri"
        ) ? (
          $Uri
        ) : (
          (Test-Path $Path) ? (Resolve-Path $Path) : ($Path)
        )
      )
    )
  }
}
