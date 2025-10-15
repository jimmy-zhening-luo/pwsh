New-Alias gitcl Import-Repository
<#
.SYNOPSIS
Use Git to clone a repository.
.DESCRIPTION
This function is an alias for `git clone` and allows you to clone a repository into a specified path.
.LINK
https://git-scm.com/docs/git-clone
#>
function Import-Repository {
  param(
    [Parameter(Mandatory)]
    [System.String]$Repository,
    [System.String]$Path,
    [Alias("fs", "ssh", "sh", "git")]
    [switch]$ForceSsh,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $RepoParts = $Repository.Trim() -split '/' |
    % { $_.Trim() } |
    ? { $_ -ne '' }

  if (-not $RepoParts) {
    throw "No repository name provided."
  }

  $OrgRepoParts = @()

  if ($RepoParts.Count -eq 1) {
    $OrgRepoParts += 'jimmy-zhening-luo'
  }

  $OrgRepoParts += $RepoParts

  $GitArguments = , (($ForceSsh ? "git@github.com:" : "https://github.com/") + ($OrgRepoParts -join '/'))

  if ($Path -and $Path.StartsWith('-')) {
    $GitArguments += $Path
    $Path = ""
  }

  $Clone = @{
    Path      = $Path
    Verb      = 'clone'
    ErrorStop = $ErrorStop
  }

  Invoke-Repository @Clone $GitArguments @args
}
