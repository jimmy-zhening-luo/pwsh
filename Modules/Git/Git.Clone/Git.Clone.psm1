New-Alias gitcl Git\Import-Repository
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

  $Local:args = $args

  if ($Path.StartsWith('-')) {
    $Local:args = , $Path + $Local:args
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
  $Local:args = , $Origin + $Local:args
  $Clone = @{
    Path  = $Path
    Verb  = 'clone'
    Throw = $Throw
  }
  Invoke-Repository @Clone @Local:args
}
