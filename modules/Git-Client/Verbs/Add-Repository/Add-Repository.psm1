Import-Module "$(Split-Path (Split-Path $PSScriptRoot))\Invoke-Repository"

New-Alias gita Add-Repository
function Add-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb add .
}

Export-ModuleMember Add-Repository -Alias gita
