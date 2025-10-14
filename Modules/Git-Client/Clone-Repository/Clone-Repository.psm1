New-Alias gitcl Import-Repository
<#
.SYNOPSIS
Use Git to clone a repository.
.DESCRIPTION
This function is an alias for `git clone` and allows you to clone a repository into a specified path.
.LINK
https://git-scm.com/docs/git-clone
#>
function Import-Repository {
  param(
    [Parameter(Mandatory)]
    [System.String]$Repository,
    [System.String]$Path = $CODE,
    [Alias("fs", "ssh", "sh", "git")]
    [switch]$ForceSsh
  )

  $Parts = $Repository.Trim() -split '/' |
    % { $_.Trim() } |
    ? { $_ -ne '' }

  if ($Parts) {
    $OrganizationParts = $()

    if ($Parts.Count -eq 1) {
      $OrganizationParts += 'jimmy-zhening-luo'
    }

    $OrganizationParts += $Parts

    $RepositoryUrl = ($ForceSsh ? "git@github.com:" : "https://github.com/") + ($OrganizationParts -join '/')

    if (Test-Path $Path -PathType Container) {
      git -C $Path clone $RepositoryUrl
    }
    else {
      throw "Path '$Path' is not a directory."
    }
  }
  else {
    throw "No repository name provided."
  }
}
