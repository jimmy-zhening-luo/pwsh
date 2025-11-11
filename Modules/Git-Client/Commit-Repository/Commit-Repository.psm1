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
    [Alias('empty', 'ae')]
    [switch]$AllowEmpty,
    [Alias('Stop', 'es')]
    [switch]$ErrorStop
  )

  $CommitArguments, $Messages = $args.Where(
    { $_ -and $_ -is [string] }
  ).Where(
    { $_.StartsWith('--') },
    'Split'
  )

  if ($Message) {
    if ($Message.StartsWith('--')) {
      $CommitArguments = , $Message + $CommitArguments
    }
    else {
      $Messages = , $Message + $Messages
    }
  }

  if ($Path) {
    if (-not (Resolve-Repository -Path $Path)) {
      if ($Path.StartsWith('--')) {
        $CommitArguments = , $Path + $CommitArguments
      }
      else {
        $Messages = , $Path + $Messages
      }

      $Path = ''
    }
  }

  $AllowEmptyFull = '--allow-empty'

  if ($AllowEmpty) {
    if ($AllowEmptyFull -notin $CommitArguments) {
      $CommitArguments += $AllowEmptyFull
    }
  }
  else {
    if ($AllowEmptyFull -in $CommitArguments) {
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

  $Add = @{
    Path      = $Path
    ErrorStop = $true
  }
  $Commit = @{
    Path      = $Path
    Verb      = 'commit'
    ErrorStop = $ErrorStop
  }

  $MessageString = $Messages -join ' '

  (
    Add-Repository @Add
  ) && (
    Invoke-Repository @Commit -m $MessageString @CommitArguments
  )
}
