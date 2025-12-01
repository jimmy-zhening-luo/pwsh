New-Alias go Open-Url
New-Alias open Open-Url
function Open-Url {
  [CmdletBinding(DefaultParameterSetName = 'Path')]
  param(
    [Parameter(
      ParameterSetName = 'Path',
      Position = 0
    )]
    [string]$Path = '.',
    [Parameter(
      ParameterSetName = 'Uri',
      Position = 0,
      Mandatory
    )]
    [Uri]$Uri
  )

  $Argument = $PSCmdlet.ParameterSetName -eq 'Uri' ? $Uri : (
    (
      Test-Path $Path
    ) ? (
      Resolve-Path $Path
    ).Path : $Path
  )

  $Browser = @{
    FilePath     = 'C:\Program Files\Google\Chrome\Application\chrome.exe'
    ArgumentList = $Argument
  }

  if (-not $env:SSH_CLIENT) {
    [void](Start-Process @Browser)
  }
}
