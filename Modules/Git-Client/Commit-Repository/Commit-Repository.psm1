New-Alias gitm Write-Repository
New-Alias ggm Write-Repository
<#
.SYNOPSIS
Commit changes to a Git repository.
.DESCRIPTION
This function commits changes to a Git repository using the 'git commit' command.
.LINK
https://git-scm.com/docs/git-commit
#>
function Write-Repository {
  param(
    [string]$Path,
    [string]$Message,
    [Alias("empty", "ae")]
    [switch]$AllowEmpty,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  if ($Path) {
    if (-not $Message) {
      if (-not (Resolve-Repository -Path $Path)) {
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
    ErrorStop = $true
  }
  $Commit = @{
    Path      = $Path
    Verb      = "commit"
    ErrorStop = $ErrorStop
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
