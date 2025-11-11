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
    [Alias('fs', 'ssh', 'sh')]
    [switch]$ForceSsh,
    [switch]$StopError
  )

  $RepositoryPath = $Repository -split '/' |
    % { $_.Trim() } |
    ? { -not [string]::IsNullOrEmpty($_) }

  if (-not $RepositoryPath) {
    throw 'No repository name given.'
  }

  if ($RepositoryPath.Count -eq 1) {
    $RepositoryPath = , 'jimmy-zhening-luo' + $RepositoryPath
  }

  $Scheme = $ForceSsh ? 'git@github.com:' : 'https://github.com/'

  $GitArguments = , ($Scheme + ($RepositoryPath -join '/'))

  if ($Path -and $Path.StartsWith('-')) {
    $GitArguments += $Path
    $Path = ''
  }

  $GitArguments += $args

  $Clone = @{
    Path      = $Path
    Verb      = 'clone'
    StopError = $StopError
  }

  Invoke-Repository @Clone @GitArguments
}
