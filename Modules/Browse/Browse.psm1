New-Alias tn Browse\Test-Host
function Test-Host {
  param(
    [Alias('ComputerName', 'RemoteAddress', 'cn')]
    [string]$HostName,
    [Alias('RemotePort')]
    [string]$Port
  )

  if ($Hostname -match '^\s*\d{1,5}\s*$' -and $Hostname -as [UInt16]) {
    if ($Port) {
      $Hostname, $Port = $Port, $Hostname
    }
    else {
      throw 'No hostname provided'
    }
  }

  $Target = @{
    ComputerName = $HostName
  }

  $Local:args = $args

  if ($Port) {
    if ($Port -as [UInt16]) {
      $Target.Port = [UInt16]$Port
    }
    else {
      $Local:args = , $Port + $Local:args
    }
  }

  Test-NetConnection @Target @Local:args
}

New-Alias tu Browse\Test-Url
<#
.SYNOPSIS
Determine if an URL is reachable.
.DESCRIPTION
This function checks if a given URL is reachable by sending a web request and checking the status code of the response. It returns '$true' if the URL is reachable (status code 200) and '$false' otherwise.
.PARAMETER Uri
Specifies the URL to test. This parameter is mandatory and must be castable to a 'Uri' object. If the URL has no scheme, it defaults to 'http'.
#>
function Test-Url {
  param(
    [Uri]$Uri
  )

  if (-not $Uri) {
    return $false
  }

  $Request = @{
    Uri                          = $Uri
    Method                       = 'HEAD'
    PreserveHttpMethodOnRedirect = $true
    DisableKeepAlive             = $true
    ConnectionTimeoutSeconds     = 5
    MaximumRetryCount            = 0
    ErrorAction                  = 'Stop'
  }

  try {
    $Status = Invoke-WebRequest @Request |
      Select-Object -ExpandProperty StatusCode
  }
  catch {
    $Status = $_.Exception.Response.StatusCode.value__
  }

  $Status -ge 200 -and $Status -lt 300
}

New-Alias go Browse\Open-Url
New-Alias open Browse\Open-Url
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
