New-Alias gitp Get-Repository
function Get-Repository {
  param(
    [System.String]$Path
  )
  Invoke-Repository -Path $Path -Verb pull
}

New-Alias gitpa Get-ChildRepository
New-Alias gpa Get-ChildRepository
function Get-ChildRepository {
  Get-ChildItem -Path $code -Directory
  | Where-Object { Resolve-Repository $_.FullName }
  | ForEach-Object { Invoke-Repository -Path $_.FullName -Verb pull }
}
