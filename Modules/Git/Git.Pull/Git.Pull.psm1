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
  $Code = @{
    Path      = "$HOME\code"
    Directory = $True
  }
  $Repositories = Get-ChildItem @Code |
    Select-Object -ExpandProperty FullName |
    % { Resolve-Repository $_ }
    ? { $_ }
  $Count = $Repositories.Count

  foreach ($Repository in $Repositories) {
    Get-Repository -Path $Repository @args
  }

  "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}
