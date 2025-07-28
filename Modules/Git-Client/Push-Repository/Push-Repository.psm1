New-Alias gits Push-Repository
function Push-Repository {
  param([System.String]$Path)

  Invoke-Repository -Path $Path -Verb push
}
