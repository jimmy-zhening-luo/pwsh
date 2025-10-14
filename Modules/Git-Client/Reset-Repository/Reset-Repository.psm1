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
    if ($Tree -as "System.UInt32") {
      $Tree = "HEAD~$($Tree -as 'System.UInt32')"
    }
    elseif ($Tree.StartsWith("~")) {
      if ($Tree -eq "~") {
        $Tree = "HEAD~"
      }
      elseif ($Tree.Substring(1) -as "System.UInt32") {
        $Tree = "HEAD~$($Tree.Substring(1) -as 'System.UInt32')"
      }
    }
    elseif ($Tree.StartsWith("^")) {
      if ($Tree -eq "^") {
        $Tree = "HEAD^"
      }
      elseif ($Tree.Substring(1) -as "System.UInt32") {
        $Tree = "HEAD^$($Tree.Substring(1) -as 'System.UInt32')"
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
        elseif ($Tree.Substring(5) -as "System.UInt32") {
          $Tree = "HEAD~$($Tree.Substring(5) -as 'System.UInt32')"
        }
      }
      elseif ($Tree[4] -eq "^") {
        if ($Tree.Length -eq 5) {
          $Tree = "HEAD^"
        }
        elseif ($Tree.Substring(5) -as "System.UInt32") {
          $Tree = "HEAD^$($Tree.Substring(5) -as 'System.UInt32')"
        }
      }
      elseif ($Tree.Substring(4) -as "System.UInt32") {
        $Tree = "HEAD~$($Tree.Substring(4) -as "System.UInt32")"
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
  param([System.String]$Path)

  $Reset = @{
    Path      = $Path
    ErrorStop = $true
  }

  (Reset-Repository @Reset) && (Get-Repository -Path $Path)
}
