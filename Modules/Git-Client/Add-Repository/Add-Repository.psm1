New-Alias gita Add-Repository
function Add-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb add .
}
