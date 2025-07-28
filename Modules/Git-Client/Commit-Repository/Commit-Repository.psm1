New-Alias gitm Write-Repository
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
    [System.String]$Message
  )

  if (-not $Message) {
    $Message = $Path
    $Path = $null
  }

  (Add-Repository -Path $Path -ErrorAction Stop) && (Invoke-Repository -Path $Path -Verb commit -m $Message)
}
