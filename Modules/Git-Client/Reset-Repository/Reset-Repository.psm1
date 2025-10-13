New-Alias gitr Undo-Repository
New-Alias gr Undo-Repository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for `git add . && git reset --hard`.
.LINK
https://git-scm.com/docs/git-reset
#>
function Undo-Repository {
  param([System.String]$Path)

  (Add-Repository -Path $Path -ErrorAction Stop) && (Invoke-Repository -Path $Path -Verb reset --hard @args)
}

New-Alias gitrp Restore-Repository
New-Alias grp Restore-Repository
<#
.SYNOPSIS
Use Git to restore a repository to its previous state.
.DESCRIPTION
This function is an alias for `git add . && git reset --hard && git pull`.
.LINK
https://git-scm.com/docs/git-reset
.LINK
https://git-scm.com/docs/git-pull
#>
function Restore-Repository {
  param([System.String]$Path)

  (Undo-Repository -Path $Path) && (Get-Repository -Path $Path)
}
