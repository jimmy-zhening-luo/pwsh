Import-Module "$(Split-Path (Split-Path $PSScriptRoot))\Invoke-Repository"

New-Alias gitp Get-Repository
function Get-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb pull
}

New-Alias gitpa Get-ChildRepository
function Get-ChildRepository {
  Get-ChildItem -Path $code -Directory
  | Where-Object { Resolve-Repository $_.FullName }
  | ForEach-Object { Invoke-Repository -Path $_.FullName -Verb pull }
}

Export-ModuleMember Get-Repository, Get-ChildRepository -Alias gitp, gitpa
