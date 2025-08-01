New-Alias gits Push-Repository
<#
.SYNOPSIS
Use Git to push changes to a repository.
.DESCRIPTION
This function is an alias for `git push`.
.LINK
https://git-scm.com/docs/git-push
#>
function Push-Repository {
  param([System.String]$Path)

  Invoke-Repository -Path $Path -Verb push
}
