Import-Module "$(Split-Path (Split-Path $PSScriptRoot))\Invoke-Repository"

New-Alias gitcp Push-Repository
function Push-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb push
}

Export-ModuleMember Push-Repository -Alias gitcp
