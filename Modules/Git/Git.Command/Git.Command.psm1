New-Alias gitcl Git\Import-GitRepository
<#
.SYNOPSIS
Use Git to clone a repository.
.DESCRIPTION
This function is an alias for 'git clone' and allows you to clone a repository into a specified path.
.LINK
https://git-scm.com/docs/git-clone
#>
function Import-GitRepository {
  param(
    # Remote repository URL or 'org/repo'
    [string]$Repository,
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw,
    [Alias('ssh')]
    # Use git@github.com remote protocol instead of the default HTTPS
    [switch]$ForceSsh
  )

  $GitCommandArguments = $args

  if ($Path.StartsWith('-')) {
    $GitCommandArguments = , $Path + $GitCommandArguments
    $Path = ''
  }

  $RepositoryPathParts = $Repository -split '/' -notmatch '^\s*$'

  if (-not $RepositoryPathParts) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathParts.Count -eq 1) {
    $RepositoryPathParts = , 'jimmy-zhening-luo' + $RepositoryPathParts
  }

  $Protocol = $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  $Origin = $Protocol + ($RepositoryPathParts -join '/')
  $GitCommandArguments = , $Origin + $GitCommandArguments
  $Clone = @{
    Path  = $Path
    Verb  = 'clone'
    Throw = $Throw
  }
  Invoke-GitRepository @Clone @GitCommandArguments
}

<#
.SYNOPSIS
Use Git to pull changes from a repository.
.DESCRIPTION
This function is an alias for 'git pull [arguments]'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $Pull = @{
    Verb = 'pull'
  }
  Invoke-GitRepository @Pull @PSBoundParameters @args
}

New-Alias gpp Git\Get-ChildGitRepository
<#
.SYNOPSIS
Use Git to pull changes for all repositories in the top level of %USERPROFILE%\code'.
.DESCRIPTION
This function runs 'git pull [arguments]' in each child repository in %USERPROFILE%\code'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildGitRepository {
  $Code = @{
    Path      = "$HOME\code"
    Directory = $True
  }
  $Repositories = Get-ChildItem @Code |
    Select-Object -ExpandProperty FullName |
    Resolve-GitRepository
  $Count = $Repositories.Count

  foreach ($Repository in $Repositories) {
    Get-GitRepository -Path $Repository @args
  }

  "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}

New-Alias ga Git\Add-GitRepository
<#
.SYNOPSIS
Use Git to stage all changes in a repository.
.DESCRIPTION
This function is an alias for 'git add [.]' and stages all changes in the repository.
.LINK
https://git-scm.com/docs/git-add
#>
function Add-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # File pattern of files to add, defaults to '.' (all)
    [string]$Name = '.',
    # Stop execution on Git error
    [switch]$Throw,
    # Include '--renormalize' flag
    [switch]$Renormalize
  )

  $GitCommandArguments = $args
  if ($Name) {
    $GitCommandArguments = , $Name + $GitCommandArguments
  }
  if ($Renormalize -and '--renormalize' -notin $args) {
    $GitCommandArguments += '--renormalize'
  }

  $Add = @{
    Path  = $Path
    Verb  = 'add'
    Throw = $Throw
  }
  Invoke-GitRepository @Add @GitCommandArguments
}

<#
.SYNOPSIS
Commit changes to a Git repository.
.DESCRIPTION
This function commits changes to a Git repository using the 'git commit' command.
.LINK
https://git-scm.com/docs/git-commit
#>
function Write-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # Commit message. It must be non-empty except on an empty commit, where it defaults to 'No message.'
    [string]$Message,
    # Stop execution on Git error
    [switch]$Throw,
    # Only commit files that are already staged
    [switch]$Staged,
    # Allow empty commit ('--allow-empty')
    [switch]$AllowEmpty
  )

  $GitCommandArguments = $args

  if ($Message) {
    $GitCommandArguments = , $Message + $GitCommandArguments
  }

  $CommitArguments, $Messages = $GitCommandArguments.Where(
    { $_ -and $_ -is [string] }
  ).Where(
    { $_ -match '^-(?>\w|-\w+)$' },
    'Split'
  )

  if ($Path) {
    if (-not ($Path | Resolve-GitRepository)) {
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
      $AllowEmpty = $True
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
  if (-not $Staged) {
    Add-GitRepository @Parameters -Throw
  }

  $CommitArguments = '-m', ($Messages -join ' ') + $CommitArguments
  $Commit = @{
    Verb = 'commit'
  }
  Invoke-GitRepository @Commit @Parameters @CommitArguments
}

New-Alias gs Git\Push-GitRepository
<#
.SYNOPSIS
Use Git to push changes to a repository.
.DESCRIPTION
This function is an alias for 'git push'.
.LINK
https://git-scm.com/docs/git-push
#>
function Push-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  Get-GitRepository @PSBoundParameters -Throw

  $Push = @{
    Verb = 'push'
  }
  Invoke-GitRepository @Push @PSBoundParameters @args
}

New-Alias gr Git\Reset-GitRepository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [HEAD]([~]|^)[n]'.
.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # The tree spec to which to revert, specified as '[HEAD]([~]|^)[n]'. If the tree spec is not specified, it defaults to HEAD. If only the number index is given, it defaults to '~' branching. If only the branching is given, the index defaults to 0 = HEAD.
    [string]$Tree,
    # Stop execution on Git error
    [switch]$Throw,
    # Remove --hard flag (no destructive reset)
    [switch]$Soft
  )

  if ($Path -eq '~' -and -not $Tree -or -not ($Path | Resolve-GitRepository)) {
    $Path, $Tree = '', $Path
  }

  $Parameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-GitRepository @Parameters -Throw

  $GitCommandArguments = $args

  if ($Tree) {
    if ($Tree -match '^(?>head)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))' -and (-not $Matches.Step -or $Matches.Step -as [uint32])) {
      $Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }

    $GitCommandArguments = , $Tree + $GitCommandArguments
  }

  $GitCommandArguments = , '--hard' + $GitCommandArguments
  $Reset = @{
    Verb = 'reset'
  }
  Invoke-GitRepository @Reset @Parameters @GitCommandArguments
}

New-Alias grp Git\Restore-GitRepository
<#
.SYNOPSIS
Use Git to restore a repository to its previous state.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.
.LINK
https://git-scm.com/docs/git-reset
.LINK
https://git-scm.com/docs/git-pull
#>
function Restore-GitRepository {
  param(
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  Reset-GitRepository @PSBoundParameters -Throw @args
  Get-GitRepository @PSBoundParameters
}
