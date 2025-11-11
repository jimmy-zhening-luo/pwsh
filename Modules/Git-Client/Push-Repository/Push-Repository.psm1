New-Alias gits Push-Repository
New-Alias ggs Push-Repository
<#
.SYNOPSIS
Use Git to push changes to a repository.
.DESCRIPTION
This function is an alias for 'git push'.
.LINK
https://git-scm.com/docs/git-push
#>
function Push-Repository {
  param(
    [string]$Path,
    [switch]$StopError
  )

  $Push = @{
    Path      = $Path
    Verb      = 'push'
    StopError = $StopError
  }

  Invoke-Repository @Push @args
}
