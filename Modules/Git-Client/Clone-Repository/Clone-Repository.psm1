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
  [CmdletBinding(SupportsShouldProcess)]
  param(
    [Parameter(
      Position = 0,
      Mandatory
    )]
    [System.String]$Repository,
    [Parameter(Position = 1)]
    [System.String]$Path = $CODE,
    [switch]$ForceSsh
  )

  $Segments = $Repository.Trim() -split '/' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne "" }

  if ($Segments.Count -eq 0) {
    throw "Empty repository name provided."
  }
  else {
    $RepositoryPath = ($Segments.Count -eq 1 ? "jimmy-zhening-luo/" : "") + ($Segments -join "/")
    $RepositoryUrl = ($ForceSsh ? "git@github.com:" : "https://github.com/") + $RepositoryPath + ($RepositoryPath.EndsWith(".git") ? "" : ".git")

    if (
      $PSCmdlet.ShouldProcess(
        $RepositoryUrl,
        "git clone -C $Path"
      )
    ) {
      if (Test-Path $Path -PathType Container) {
        git -C $Path clone $RepositoryUrl
      }
      else {
        throw "The specified path '$Path' is not a directory."
      }
    }
  }
}
