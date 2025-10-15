New-Alias gitp Get-Repository
New-Alias ggp Get-Repository
<#
.SYNOPSIS
Use Git to pull changes from a repository.
.DESCRIPTION
This function is an alias for `git pull`.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-Repository {
  param(
    [System.String]$Path,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $Pull = @{
    Path      = $Path
    Verb      = "pull"
    ErrorStop = $ErrorStop
  }

  Invoke-Repository @Pull @args
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
  $Pull = @{
    Verb = "pull"
  }

  $Repositories = Get-ChildItem $CODE -Directory |
    ? { Resolve-Repository $_.FullName }

  $Repositories |
    % { Invoke-Repository $_.FullName @Pull @args }

  Write-Output "`nPulled $(Format-Count repository/repositories $Repositories.Count)."
}
