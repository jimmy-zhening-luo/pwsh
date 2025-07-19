<#
.SYNOPSIS
Determine if an URL is reachable.

.DESCRIPTION
This function checks if a given URL is reachable by sending a web request and checking the status code of the response. It returns `$true` if the URL is reachable (status code 200) and `$false` otherwise.

.PARAMETER Uri
Specifies the URL to test. This parameter is mandatory and must be castable to a `Uri` object. If the URL has no scheme, it defaults to `http`.
#>
function Test-Url {
  param(
    [Parameter(Mandatory)]
    [Uri]$Uri
  )

  try {
    $StatusCode = (Invoke-WebRequest -Uri $Uri).StatusCode
  }
  catch {
    $StatusCode = $_.Exception.Response.StatusCode.value__
  }
  return $StatusCode -eq 200
}
