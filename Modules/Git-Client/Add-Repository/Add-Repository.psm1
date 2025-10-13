New-Alias gita Add-Repository
New-Alias gga Add-Repository
<#
.SYNOPSIS
Use Git to stage all changes in a repository.
.DESCRIPTION
This function is an alias for `git add .` and stages all changes in the repository.
.LINK
https://git-scm.com/docs/git-add
#>
function Add-Repository {
  param(
    [System.String]$Path,
    [Alias("r")]
    [switch]$Renormalize
  )

  $Required = @{
    Path       = $Path
    Verb       = "add"
  }

  $GitOptions = , "."

  if ($Renormalize) {
    $GitOptions += "--renormalize"
  }

  Invoke-Repository @Required $GitOptions @args
}
