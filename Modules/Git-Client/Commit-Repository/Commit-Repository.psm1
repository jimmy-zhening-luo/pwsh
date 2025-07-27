New-Alias gitm Write-Repository
function Write-Repository {
  param(
    [System.String]$Path,
    [System.String]$Message
  )

  if (-not $Message) {
    $Message = $Path
    $Path = $null
  }

  (Add-Repository -Path $Path -ErrorAction Stop) && (Invoke-Repository -Path $Path -Verb commit -m $Message)
}
