New-Alias gitcp Push-Repository
function Push-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb push
}

Export-ModuleMember Push-Repository -Alias gitcp
