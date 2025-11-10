New-Alias gita Add-Repository
New-Alias gga Add-Repository
<#
.SYNOPSIS
Use Git to stage all changes in a repository.
.DESCRIPTION
This function is an alias for 'git add .' and stages all changes in the repository.
.LINK
https://git-scm.com/docs/git-add
#>
function Add-Repository {
  param(
    [string]$Path,
    [Alias("r")]
    [switch]$Renormalize,
    [Alias("Stop", "es")]
    [switch]$ErrorStop
  )

  $Add = @{
    Path      = $Path
    Verb      = "add"
    ErrorStop = $ErrorStop
  }
  $GitArguments = , "."

  if ($Renormalize) {
    $GitArguments += "--renormalize"
  }

  Invoke-Repository @Add $GitArguments @args
}
