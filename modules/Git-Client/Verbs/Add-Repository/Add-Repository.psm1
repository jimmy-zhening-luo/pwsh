New-Alias gita Add-Repository
function Add-Repository {
  param(
    [string]$Path
  )
  echo $Path
  # Invoke-Repository -Path $Path -Verb add .
}

Export-ModuleMember Add-Repository -Alias gita
