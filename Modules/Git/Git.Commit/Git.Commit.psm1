New-Alias ggm Git\Write-Repository
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
    [switch]$Throw,
    [switch]$AllowEmpty
  )

  $Local:args = $args

  if ($Message) {
    $Local:args = , $Message + $Local:args
  }

  $CommitArguments, $Messages = $Local:args.Where(
    { $_ -and $_ -is [string] }
  ).Where(
    { $_ -match '^-(?>\w|-\w+)$' },
    'Split'
  )

  if ($Path) {
    if (-not (Resolve-Repository -Path $Path)) {
      if ($Path -match '^-(?>\w|-\w+)$') {
        $CommitArguments = , $Path + $CommitArguments
      }
      else {
        $Messages = , $Path + $Messages
      }

      $Path = ''
    }
  }

  $fAllowEmpty = '--allow-empty'

  if ($AllowEmpty) {
    if ($fAllowEmpty -notin $CommitArguments) {
      $CommitArguments += $fAllowEmpty
    }
  }
  else {
    if ($fAllowEmpty -in $CommitArguments) {
      $AllowEmpty = $true
    }
  }

  if (-not $Messages) {
    if ($AllowEmpty) {
      $Messages += 'No message.'
    }
    else {
      throw 'Missing commit message.'
    }
  }

  $Parameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-Repository @Parameters -Throw

  $CommitArguments = '-m', ($Messages -join ' ') + $CommitArguments
  $Commit = @{
    Verb = 'commit'
  }
  Invoke-Repository @Commit @Parameters @CommitArguments
}
