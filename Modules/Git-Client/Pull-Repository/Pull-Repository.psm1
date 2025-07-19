New-Alias -Name gitp -Value Get-Repository -Option ReadOnly
function Get-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb pull
}

New-Alias -Name gitpa -Value Get-ChildRepository -Option ReadOnly
function Get-ChildRepository {
  Get-ChildItem -Path $code -Directory
  | Where-Object { Resolve-Repository $_.FullName }
  | ForEach-Object { Invoke-Repository -Path $_.FullName -Verb pull }
}
