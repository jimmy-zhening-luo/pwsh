New-Alias -Name gita -Value Add-Repository
function Add-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb add .
}
