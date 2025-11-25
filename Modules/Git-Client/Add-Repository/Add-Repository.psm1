New-Alias gita Add-Repository
New-Alias gga Add-Repository
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

  $All = '.'
  $fRenormalize = '--renormalize'

  if ($All -notin $args) {
    $args = , $All + $args
  }

  if ($Renormalize -and $fRenormalize -notin $args) {
    $args += $fRenormalize
  }

  $Add = @{
    Path  = $Path
    Verb  = 'add'
    Throw = $Throw
  }

  Invoke-Repository @Add @args
}
