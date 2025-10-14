New-Alias gitm Write-Repository
New-Alias ggm Write-Repository
<#
.SYNOPSIS
Commit changes to a Git repository.
.DESCRIPTION
This function commits changes to a Git repository using the `git commit` command.
.LINK
https://git-scm.com/docs/git-commit
#>
function Write-Repository {
  param(
    [System.String]$Path,
    [System.String]$Message,
    [Alias("empty", "ae")]
    [switch]$AllowEmpty
  )

  if ($Path) {
    if (-not $Message) {
      if (-not (Resolve-Repository $Path)) {
        $Message = $Path
        $Path = ""
      }
      elseif ($AllowEmpty) {
        $Message = "-"
      }
      else {
        throw "Missing commit message."
      }
    }
  }
  else {
    if (-not $Message) {
      if ($AllowEmpty) {
        $Message = "-"
      }
      else {
        throw "Missing commit message."
      }
    }
  }

  $Add = @{
    Path      = $Path
    StopError = $true
  }
  $Commit = @{
    Path = $Path
    Verb = "commit"
  }

  (Add-Repository @Add) && (
    (
      $AllowEmpty
    ) ? (
      Invoke-Repository @Commit -m $Message --allow-empty @args
    ) : (
      Invoke-Repository @Commit -m $Message @args
    )
  )
}
