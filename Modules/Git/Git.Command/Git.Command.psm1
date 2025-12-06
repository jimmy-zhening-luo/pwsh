New-Alias gitcl Git\Import-Repository
<#
.SYNOPSIS
Use Git to clone a repository.
.DESCRIPTION
This function is an alias for 'git clone' and allows you to clone a repository into a specified path.
.LINK
https://git-scm.com/docs/git-clone
#>
function Import-Repository {
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

  $Local:args = $args

  if ($Path.StartsWith('-')) {
    $Local:args = , $Path + $Local:args
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
  $Local:args = , $Origin + $Local:args
  $Clone = @{
    Path  = $Path
    Verb  = 'clone'
    Throw = $Throw
  }
  Invoke-Repository @Clone @Local:args
}

<#
.SYNOPSIS
Use Git to pull changes from a repository.
.DESCRIPTION
This function is an alias for 'git pull [arguments]'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-Repository {
  param(
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $Pull = @{
    Verb = 'pull'
  }
  Invoke-Repository @Pull @PSBoundParameters @args
}

New-Alias gpp Git\Get-ChildRepository
<#
.SYNOPSIS
Use Git to pull changes for all repositories in the top level of %USERPROFILE%\code'.
.DESCRIPTION
This function runs 'git pull [arguments]' in each child repository in %USERPROFILE%\code'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildRepository {
  $Code = @{
    Path      = "$HOME\code"
    Directory = $True
  }
  $Repositories = Get-ChildItem @Code |
    Select-Object -ExpandProperty FullName |
    Resolve-Repository
  $Count = $Repositories.Count

  foreach ($Repository in $Repositories) {
    Get-Repository -Path $Repository @args
  }

  "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}

New-Alias ga Git\Add-Repository
<#
.SYNOPSIS
Use Git to stage all changes in a repository.
.DESCRIPTION
This function is an alias for 'git add [.]' and stages all changes in the repository.
.LINK
https://git-scm.com/docs/git-add
#>
function Add-Repository {
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

  $Local:args = $args
  if ($Name) {
    $Local:args = , $Name + $Local:args
  }
  if ($Renormalize -and '--renormalize' -notin $args) {
    $Local:args += '--renormalize'
  }

  $Add = @{
    Path  = $Path
    Verb  = 'add'
    Throw = $Throw
  }
  Invoke-Repository @Add @Local:args
}

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
    # Local repository path
    [string]$Path,
    # Commit message. It must be non-empty except on an empty commit, where it defaults to 'No message.'
    [string]$Message,
    # Stop execution on Git error
    [switch]$Throw,
    # Allow empty commit ('--allow-empty')
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
    if (-not ($Path | Resolve-Repository)) {
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
  Add-Repository @Parameters -Throw

  $CommitArguments = '-m', ($Messages -join ' ') + $CommitArguments
  $Commit = @{
    Verb = 'commit'
  }
  Invoke-Repository @Commit @Parameters @CommitArguments
}

New-Alias gs Git\Push-Repository
<#
.SYNOPSIS
Use Git to push changes to a repository.
.DESCRIPTION
This function is an alias for 'git push'.
.LINK
https://git-scm.com/docs/git-push
#>
function Push-Repository {
  param(
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  Get-Repository @PSBoundParameters -Throw

  $Push = @{
    Verb = 'push'
  }
  Invoke-Repository @Push @PSBoundParameters @args
}

New-Alias gr Git\Reset-Repository
<#
.SYNOPSIS
Use Git to undo changes in a repository.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [HEAD]([~]|^)[n]'.
.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-Repository {
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

  if ($Path -eq '~' -and -not $Tree -or -not ($Path | Resolve-Repository)) {
    $Path, $Tree = '', $Path
  }

  $Parameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-Repository @Parameters -Throw

  $Local:args = $args

  if ($Tree) {
    if ($Tree -match '^(?>head)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))' -and (-not $Matches.Step -or $Matches.Step -as [uint32])) {
      $Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }

    $Local:args = , $Tree + $Local:args
  }

  $Local:args = , '--hard' + $Local:args
  $Reset = @{
    Verb = 'reset'
  }
  Invoke-Repository @Reset @Parameters @Local:args
}

New-Alias grp Git\Restore-Repository
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
function Restore-Repository {
  param(
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  Reset-Repository @PSBoundParameters -Throw @args
  Get-Repository @PSBoundParameters
}
