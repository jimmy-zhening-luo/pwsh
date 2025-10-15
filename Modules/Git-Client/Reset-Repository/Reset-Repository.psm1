New-Alias gitr Reset-Repository
New-Alias gr Reset-Repository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for `git add . && git reset --hard [[[HEAD]~][n=1]]`.
.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-Repository {
  param(
    [System.String]$Path,
    [System.String]$Tree,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  if ($Path -and (-not $Tree) -and (($Path -eq "~") -or (-not (Resolve-Repository $Path)))) {
    $Tree = $Path
    $Path = ""
  }

  if ($Tree) {
    if ([UInt32]$Tree) {
      $Tree = "HEAD~$([System.UInt32]$Tree)"
    }
    elseif ($Tree.StartsWith("~")) {
      if ($Tree -eq "~") {
        $Tree = "HEAD~"
      }
      elseif ([System.UInt32]$Tree.Substring(1)) {
        $Tree = "HEAD~$([System.UInt32]$Tree.Substring(1))"
      }
    }
    elseif ($Tree.StartsWith("^")) {
      if ($Tree -eq "^") {
        $Tree = "HEAD^"
      }
      elseif ([System.UInt32]$Tree.Substring(1)) {
        $Tree = "HEAD^$([System.UInt32]$Tree.Substring(1))"
      }
    }
    elseif ($Tree.ToUpperInvariant().StartsWith("HEAD")) {
      if ($Tree.Length -eq 4) {
        $Tree = ""
      }
      elseif ($Tree[4] -eq "~") {
        if ($Tree.Length -eq 5) {
          $Tree = "HEAD~"
        }
        elseif ([System.UInt32]$Tree.Substring(5)) {
          $Tree = "HEAD~$([System.UInt32]$Tree.Substring(5))"
        }
      }
      elseif ($Tree[4] -eq "^") {
        if ($Tree.Length -eq 5) {
          $Tree = "HEAD^"
        }
        elseif ([System.UInt32]$Tree.Substring(5)) {
          $Tree = "HEAD^$([System.UInt32]$Tree.Substring(5))"
        }
      }
      elseif ([System.UInt32]$Tree.Substring(4)) {
        $Tree = "HEAD~$([System.UInt32]$Tree.Substring(4))"
      }
    }
  }

  $Add = @{
    Path      = $Path
    ErrorStop = $true
  }
  $Reset = @{
    Path      = $Path
    Verb      = "reset"
    ErrorStop = $ErrorStop
  }
  $GitArguments = , "--hard"

  if ($Tree) {
    $GitArguments += $Tree
  }

  (Add-Repository @Add) && (Invoke-Repository @Reset $GitArguments @args)
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
  param(
    [System.String]$Path,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $Reset = @{
    Path      = $Path
    ErrorStop = $true
  }
  $Pull = @{
    Path      = $Path
    ErrorStop = $ErrorStop
  }

  (Reset-Repository @Reset) && (Get-Repository @Pull)
}
