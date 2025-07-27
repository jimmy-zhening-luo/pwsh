New-Alias gita Add-Repository
function Add-Repository {
  param([System.String]$Path)

  Invoke-Repository -Path $Path -Verb add .
}
