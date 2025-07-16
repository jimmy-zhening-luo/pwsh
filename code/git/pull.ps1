New-Alias gitp Get-Repository
function Get-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb pull
}
