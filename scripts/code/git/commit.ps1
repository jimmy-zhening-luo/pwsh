New-Alias gitm Write-Repository
function Write-Repository {
  param(
    [string]$Path,
    [string]$Message
  )
  if (-not $Message) {
    $Message = $Path
    $Path = $null
  }

  try {
    Add-Repository -Path $Path -ErrorAction Stop
    Invoke-Repository -Path $Path -Verb commit -m $Message
  }
  catch {
    throw ("Failed to commit changes to repository at '$Path' with message '$Message'. Caught error: " + $_.Exception.Message)
  }
}
