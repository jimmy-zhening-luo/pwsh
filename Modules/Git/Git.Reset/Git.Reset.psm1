New-Alias gr Git\Reset-Repository
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

  if ($Path -eq '~' -and -not $Tree -or -not ($Path | Resolve-Repository)) {
    $Path, $Tree = '', $Path
  }

  $Parameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-Repository @Parameters -Throw

  $Local:args = $args

  if ($Tree) {
    if ($Tree -match '^(?>head)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))' -and (-not $Matches.Step -or $Matches.Step -as [uint32])) {
      $Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }

    $Local:args = , $Tree + $Local:args
  }

  $Local:args = , '--hard' + $Local:args
  $Reset = @{
    Verb = 'reset'
  }
  Invoke-Repository @Reset @Parameters @Local:args
}

New-Alias grp Git\Restore-Repository
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

  Reset-Repository @PSBoundParameters -Throw @args
  Get-Repository @PSBoundParameters
}
