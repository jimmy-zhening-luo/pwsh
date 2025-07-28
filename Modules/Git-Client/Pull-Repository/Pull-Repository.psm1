New-Alias gitp Get-Repository
<#
.SYNOPSIS
Use Git to pull changes from a repository.
.DESCRIPTION
This function is an alias for `git pull`.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-Repository {
  param([System.String]$Path)

  Invoke-Repository -Path $Path -Verb pull
}

New-Alias gpa Get-ChildRepository
<#
.SYNOPSIS
Use Git to pull changes from all child repositories.
.DESCRIPTION
This function retrieves all child repositories in `~\code\` and pulls changes from each one.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildRepository {
  Get-ChildItem -Path $code -Directory |
    Where-Object { Resolve-Repository $_.FullName } |
    ForEach-Object { Invoke-Repository -Path $_.FullName -Verb pull }
}
