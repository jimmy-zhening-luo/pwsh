New-Alias -Name gitcp -Value Push-Repository -Option ReadOnly
function Push-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb push
}
