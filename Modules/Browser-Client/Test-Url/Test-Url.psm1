New-Alias tu Test-Url
<#
.SYNOPSIS
Determine if an URL is reachable.
.DESCRIPTION
This function checks if a given URL is reachable by sending a web request and checking the status code of the response. It returns '$true' if the URL is reachable (status code 200) and '$false' otherwise.
.PARAMETER Uri
Specifies the URL to test. This parameter is mandatory and must be castable to a 'Uri' object. If the URL has no scheme, it defaults to 'http'.
#>
function Test-Url {
  param([Uri]$Uri)

  if (-not $Uri) {
    return $false
  }

  $Request = @{
    Uri                          = $Uri
    Method                       = 'HEAD'
    PreserveHttpMethodOnRedirect = $true
    DisableKeepAlive             = $true
    ConnectionTimeoutSeconds     = 1
    MaximumRetryCount            = 0
    ErrorAction                  = 'Stop'
  }

  try {
    $Status = (
      Invoke-WebRequest @Request
    ).StatusCode
  }
  catch {
    $Status = $_.Exception.Response.StatusCode.value__
  }

  $Status -ge 200 -and $Status -lt 300
}
