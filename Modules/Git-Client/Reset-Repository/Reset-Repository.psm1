New-Alias gitr Reset-Repository
New-Alias gr Reset-Repository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [[[HEAD]~][n=1]]'.
.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-Repository {
  param(
    [string]$Path,
    [string]$Tree,
    [switch]$Throw
  )

  if (
    $Path -and -not $Tree -and (
      $Path -eq '~' -or -not (
        Resolve-Repository -Path $Path
      )
    )
  ) {
    $Path, $Tree = '', $Path
  }

  if ($Tree) {
    if (
      $Tree -match '^(?>head)?(?<Branching>\^|~|)(?<Step>\d{0,10})' -and (
        -not $Matches.Step -or $Matches.Step -as [uint32]
      )
    ) {
      $Tree = 'HEAD' + $Matches.Branching + $Matches.Step
    }
    else {
      $args = , $Tree + $args
      $Tree = ''
    }
  }

  $args = , '--hard' + $args
  $Parameters = @{
    Path = $Path
    Throw = $Throw
  }
  $Reset = @{ Verb = 'reset' }

  Add-Repository @Parameters -Throw && Invoke-Repository @Parameters @Reset @args
}

New-Alias gitrp Restore-Repository
New-Alias grp Restore-Repository
<#
.SYNOPSIS
Use Git to restore a repository to its previous state.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.
.LINK
https://git-scm.com/docs/git-reset
.LINK
https://git-scm.com/docs/git-pull
#>
function Restore-Repository {
  param(
    [string]$Path,
    [switch]$Throw
  )

  Reset-Repository @PSBoundParameters -Throw @args && Get-Repository @PSBoundParameters
}
