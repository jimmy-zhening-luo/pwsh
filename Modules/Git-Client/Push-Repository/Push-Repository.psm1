New-Alias -Name gitcp -Value Push-Repository
function Push-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb push
}
