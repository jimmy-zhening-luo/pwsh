New-Alias -Name gita -Value Add-Repository -Option ReadOnly
function Add-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb add .
}
