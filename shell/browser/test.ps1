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
