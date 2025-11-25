New-Alias gitcl Import-Repository
<#
.SYNOPSIS
Use Git to clone a repository.
.DESCRIPTION
This function is an alias for 'git clone' and allows you to clone a repository into a specified path.
.LINK
https://git-scm.com/docs/git-clone
#>
function Import-Repository {
  param(
    [string]$Repository,
    [string]$Path,
    [switch]$Throw,
    [switch]$ForceSsh
  )

  if ($Path.StartsWith('-')) {
    $args = , $Path + $args
    $Path = ''
  }

  $RepositoryPathParts = $Repository -split '/' |
    ? { -not [string]::IsNullOrWhiteSpace($_) }

  if (-not $RepositoryPathParts) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathParts.Count -eq 1) {
    $RepositoryPathParts = , 'jimmy-zhening-luo' + $RepositoryPathParts
  }

  $Protocol = $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  $Origin = $Protocol + ($RepositoryPathParts -join '/')
  $args = , $Origin + $args


  $Clone = @{
    Path = $Path
    Verb = 'clone'
    Throw = $Throw
  }

  Invoke-Repository @Clone @args
}
