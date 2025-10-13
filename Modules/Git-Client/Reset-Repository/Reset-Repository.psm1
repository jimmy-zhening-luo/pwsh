New-Alias gitr Undo-Repository
New-Alias gr Undo-Repository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for `git add . && git reset --hard [[[HEAD]~][n=1]]`.
.LINK
https://git-scm.com/docs/git-reset
#>
function Undo-Repository {
  param(
    [System.String]$Path,
    [System.String]$Tree
  )

  $Option = $null

  if ($Path -and (-not $Tree) -and (-not (Test-Path -Path $Path -PathType Container))) {
    $Tree = $Path
    $Path = $null
  }

  if ($Tree) {
    if ($Tree -as "System.UInt32") {
      $Tree = "HEAD~$(Path -as "System.UInt32")"
    }
    elseif ($Tree.StartsWith("~")) {
      if ($Tree -eq "~") {
        $Tree = "HEAD~"
      }
      elseif ($Tree.Substring(1) -as "System.UInt32") {
        $Tree = "HEAD~$(Path.Substring(1) -as "System.UInt32")"
      }
      else {
        $Option = $Tree
        $Tree = $null
      }
    }
    elseif ($Tree.StartsWith("^")) {
      if ($Tree -eq "^") {
        $Tree = "HEAD^"
      }
      elseif ($Path.Substring(1) -as "System.UInt32") {
        $Tree = "HEAD^$(Path.Substring(1) -as "System.UInt32")"
      }
      else {
        $Option = $Tree
        $Tree = $null
      }
    }
    elseif ($Tree.ToUpperInvariant().StartsWith("HEAD")) {
      if ($Tree.ToUpperInvariant() -eq "HEAD") {
        $Tree = $null
      }
      elseif ($Tree[4] -eq "~") {
        if ($Tree -eq "HEAD~") {
          $Tree = "HEAD~"
        }
        elseif ($Tree.Substring(5) -as "System.UInt32") {
          $Tree = "HEAD~$(Tree.Substring(5) -as "System.UInt32")"
        }
        else {
          $Option = $Tree
          $Tree = $null
        }
      }
      elseif ($Tree[4] -eq "^") {
        if ($Tree -eq "HEAD^") {
          $Tree = "HEAD^"
        }
        elseif ($Tree.Substring(5) -as "System.UInt32") {
          $Tree = "HEAD^$(Tree.Substring(5) -as "System.UInt32")"
        }
        else {
          $Option = $Tree
          $Tree = $null
        }
      }
      elseif ($Tree.Substring(4) -as "System.UInt32") {
        $Tree = "HEAD~$(Tree.Substring(4) -as "System.UInt32")"
      }
      else {
        $Option = $Tree
        $Tree = $null
      }
    }
    else {
      $Option = $Tree
      $Tree = $null
    }
  }

  $PathSpec = @{
    Path = $Path
  }
  $Verb = @{
    Verb = "reset"
  }

  (Add-Repository @PathSpec) && (Invoke-Repository @PathSpec @Verb --hard $Tree $Option @args)
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

  $PathSpec = @{
    Path = $Path
  }

  (Undo-Repository @PathSpec) && (Get-Repository @PathSpec)
}
