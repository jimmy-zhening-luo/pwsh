New-Alias gga Git\Add-Repository
<#
.SYNOPSIS
Use Git to stage all changes in a repository.
.DESCRIPTION
This function is an alias for 'git add .' and stages all changes in the repository.
.LINK
https://git-scm.com/docs/git-add
#>
function Add-Repository {
  param(
    [string]$Path,
    [switch]$Throw,
    [Alias('r')]
    [switch]$Renormalize
  )

  $Local:args = $args
  $All = '.'
  $fRenormalize = '--renormalize'

  if ($All -notin $args) {
    $Local:args = , $All + $Local:args
  }

  if ($Renormalize -and $fRenormalize -notin $args) {
    $Local:args += $fRenormalize
  }

  $Add = @{
    Path  = $Path
    Verb  = 'add'
    Throw = $Throw
  }
  Invoke-Repository @Add @Local:args
}
