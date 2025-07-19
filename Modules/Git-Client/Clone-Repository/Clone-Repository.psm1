New-Alias -Name gitcl -Value Import-Repository -Option ReadOnly
function Import-Repository {
  [CmdletBinding(SupportsShouldProcess)]
  param (
    [Parameter(
      Position = 0,
      Mandatory
    )]
    [string]$Repository,
    [Parameter(
      Position = 1
    )]
    [string]$Path = $code,
    [switch]$Http
  )

  $Segments = $Repository.Trim() -split '/' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne "" }

  if ($Segments.Count -eq 0) {
    throw "Empty repository name provided."
  }
  else {
    $Provider = $Http ? "https://github.com/" : "git@github.com:"
    $RepositoryPath = ($Segments.Count -eq 1 ? "jimmy-zhening-luo/" : "") + ($Segments -join "/")
    $RepositoryUrl = $Provider + $RepositoryPath + ($RepositoryPath.EndsWith(".git") ? "" : ".git")

    if ($PSCmdlet.ShouldProcess($RepositoryUrl, "git clone -C $Path")) {
      if (Test-Path $Path) {
        if ((Get-Item $Path).PSIsContainer) {
          git -C $Path clone $RepositoryUrl
        }
        else {
          throw "The specified path '$Path' is not a directory."
        }
      }
      else {
        throw "The specified path '$Path' does not exist."
      }
    }
  }
}
