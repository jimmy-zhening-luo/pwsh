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
    [switch]$StopError
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
    if ($Tree -as [uint32]) {
      $Tree = "HEAD~$($Tree -as [uint32])"
    }
    elseif ($Tree.StartsWith('~')) {
      if ($Tree -eq '~') {
        $Tree = 'HEAD~'
      }
      elseif ($Tree.Substring(1) -as [uint32]) {
        $Tree = "HEAD~$($Tree.Substring(1) -as [uint32])"
      }
    }
    elseif ($Tree.StartsWith('^')) {
      if ($Tree -eq '^') {
        $Tree = 'HEAD^'
      }
      elseif ($Tree.Substring(1) -as [uint32]) {
        $Tree = "HEAD^$($Tree.Substring(1) -as [uint32])"
      }
    }
    elseif ($Tree.ToUpperInvariant().StartsWith('HEAD')) {
      if ($Tree.Length -eq 4) {
        $Tree = ''
      }
      elseif ($Tree[4] -eq '~') {
        if ($Tree.Length -eq 5) {
          $Tree = 'HEAD~'
        }
        elseif ($Tree.Substring(5) -as [uint32]) {
          $Tree = "HEAD~$($Tree.Substring(5) -as [uint32])"
        }
      }
      elseif ($Tree[4] -eq '^') {
        if ($Tree.Length -eq 5) {
          $Tree = 'HEAD^'
        }
        elseif ($Tree.Substring(5) -as [uint32]) {
          $Tree = "HEAD^$($Tree.Substring(5) -as [uint32])"
        }
      }
      elseif ($Tree.Substring(4) -as [uint32]) {
        $Tree = "HEAD~$($Tree.Substring(4) -as [uint32])"
      }
    }
  }

  $Add = @{
    Path      = $Path
    StopError = $true
  }
  $Reset = @{
    Path      = $Path
    Verb      = 'reset'
    StopError = $StopError
  }
  $ResetArguments = , '--hard'

  if ($Tree) {
    $ResetArguments += $Tree
  }

  $ResetArguments += $args

  (Add-Repository @Add) && (Invoke-Repository @Reset @ResetArguments)
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
    [switch]$StopError
  )

  $Reset = @{
    Path      = $Path
    StopError = $true
  }
  $Pull = @{
    Path      = $Path
    StopError = $StopError
  }

  (Reset-Repository @Reset @args) && (Get-Repository @Pull)
}
