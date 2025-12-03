New-Alias gitp Git\Get-Repository
New-Alias ggp Git\Get-Repository
<#
.SYNOPSIS
Use Git to pull changes from a repository.
.DESCRIPTION
This function is an alias for 'git pull'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-Repository {
  param(
    [string]$Path,
    [switch]$Throw
  )

  $Pull = @{
    Verb = 'pull'
  }
  Invoke-Repository @Pull @PSBoundParameters @args
}

New-Alias gpa Git\Get-ChildRepository
<#
.SYNOPSIS
Use Git to pull changes from all child repositories.
.DESCRIPTION
This function retrieves all child repositories in %USERPROFILE%\code\' and pulls changes from each one.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildRepository {
  $Pull = @{ Verb = 'pull' }
  $Repositories = (Get-ChildItem -Path $HOME\code -Directory).FullName |
    ? { Resolve-Repository -Path $_ } |
    ? { $_ }

  $Repositories |
    % { Invoke-Repository -Path $_ @Pull @args }
  $Count = $Repositories.Count

  "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}
