function Resolve-GitRepository {
  [CmdletBinding()]
  [OutputType([string[]])]
  param(
    [Parameter(
      Mandatory,
      Position = 0,
      ValueFromPipeline
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [PathCompletions('.', 'Directory')]
    [string[]]$Path,
    [switch]$New
  )
  begin {
    $CODE = "$HOME\code"
    $Repositories = @()
  }
  process {
    $RepoPath = @{
      Path = $Path
    }
    $Repository = ''

    if ($New) {
      if (Shell\Test-Item @RepoPath) {
        $Repository = Shell\Resolve-Item @RepoPath
      }
      else {
        $RepoPath.Location = $CODE
        $RepoPath.New = $True

        if (Shell\Test-Item @RepoPath) {
          $Repository = Shell\Resolve-Item @RepoPath
        }
      }
    }
    else {
      $RepoGitPath = @{
        Path           = $Path ? (Join-Path $Path .git) : '.git'
        RequireSubpath = $True
      }

      if (Shell\Test-Item @RepoGitPath) {
        $Repository = Shell\Resolve-Item @RepoPath
      }
      else {
        $RepoGitPath.Location = $RepoPath.Location = $CODE

        if (Shell\Test-Item @RepoGitPath) {
          $Repository = Shell\Resolve-Item @RepoPath
        }
      }
    }

    if ($Repository) {
      $Repositories += $Repository
    }
  }
  end {
    $Repositories
  }
}

$GIT_ARGUMENT = '^(?>(?=.*[*=]).+|-(?>\w|(?>-\w[-\w]*\w)))$'

<#
.SYNOPSIS
Run a Git command
.DESCRIPTION
This function allows you to run a Git command in a local repository. If no command is specified, it defaults to 'git status'. If no path is specified, it defaults to the current location. For every verb except for 'clone', the function will throw an error if there is no Git repository at the specified path.
.LINK
https://git-scm.com/docs
#>
New-Alias g Git\Invoke-GitRepository
function Invoke-GitRepository {
  param(
    [GenericCompletions(
      'add,clone,commit,config,pull,push,reset,rm,status,switch,init,diff,mv,branch,checkout,merge,stash,tag,fetch,remote,submodule,ls-files,ls-tree'
    )]
    # Git verb (command) to run
    [string]$Verb,
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $GIT_VERB_COMPLETION = (
    'add,clone,commit,config,pull,push,reset,rm,status,switch,init,diff,mv,branch,checkout,merge,stash,tag,fetch,remote,submodule,ls-files,ls-tree'
  )
  $GIT_VERB = $GIT_VERB_COMPLETION -split ','
  $NEWABLE_GIT_VERB = @(
    'clone'
    'config'
  )

  $GitArguments = $args

  if ($Verb) {
    if ($Verb -in $GIT_VERB) {
      if ($Verb -in $NEWABLE_GIT_VERB) {
        if ($Path -match $GIT_ARGUMENT) {
          $GitArguments = , $Path + $GitArguments
          $Path = ''
        }
      }
      else {
        if (
          $Path -and -not (
            $PWD | Resolve-GitRepository
          ) -and -not (
            $Path | Resolve-GitRepository
          ) -and (
            $Verb | Resolve-GitRepository
          )
        ) {
          $GitArguments = , $Path + $GitArguments
          $Verb, $Path = 'status', $Verb
        }
      }
    }
    else {
      if ($Path -or $GitArguments) {
        $GitArguments = , $Verb + $GitArguments
      }
      else {
        $Path = $Verb
      }

      $Verb = 'status'
    }
  }
  else {
    $Verb = 'status'
  }

  $Resolve = @{
    Path = $Path
    New  = $Verb -in $NEWABLE_GIT_VERB
  }
  $Repository = Resolve-GitRepository @Resolve

  if (-not $Repository) {
    if ($Path) {
      $GitArguments = , $Path + $GitArguments
    }

    $Repository = $PWD.Path
  }

  $GitArguments = @(
    '-c'
    'color.ui=always'
    '-C'
    $Repository
    $Verb
  ) + $GitArguments
  if ($Throw) {
    & git @GitArguments 2>&1 |
      Tee-Object -Variable GitResult

    if ($GitResult -match '^(?>fatal:)') {
      throw $GitResult
    }
  }
  else {
    & git @GitArguments
  }
}

New-Alias gg Git\Measure-GitRepository
<#
.SYNOPSIS
Use Git to get the status of a local repository.
.DESCRIPTION
This function is an alias for 'git status [arguments]'.
.LINK
https://git-scm.com/docs/git-status
#>
function Measure-GitRepository {
  param(
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $Status = @{
    Verb = 'status'
  }
  Invoke-GitRepository @Status @PSBoundParameters @args
}

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
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw,
    [Alias('ssh')]
    # Use git@github.com remote protocol instead of the default HTTPS
    [switch]$ForceSsh
  )

  $RepositoryPathParts = $Repository -split '/' -notmatch '^\s*$'

  if (-not $RepositoryPathParts) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathParts.Count -eq 1) {
    $RepositoryPathParts = , 'jimmy-zhening-luo' + $RepositoryPathParts
  }

  $Origin = (
    $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  ) + (
    $RepositoryPathParts -join '/'
  )
  $GitCommandArguments = , $Origin + $args
  $Clone = @{
    Verb  = 'clone'
    Path  = $Path
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
    [PathCompletions('.', 'Directory')]
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
    [PathCompletions('.', 'Directory')]
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

  if ($Name -match $GIT_ARGUMENT) {
    $GitCommandArguments = , $Name + $GitCommandArguments
    $Name = ''
  }

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    if ($Name) {
      $GitCommandArguments = , $Path + $GitCommandArguments
    }
    else {
      $Name = $Path
    }

    $Path = ''
  }

  if ($Name) {
    $GitCommandArguments = , $Name + $GitCommandArguments
  }

  if ($Renormalize -and '--renormalize' -notin $GitCommandArguments) {
    $GitCommandArguments += '--renormalize'
  }

  $Add = @{
    Verb  = 'add'
    Path  = $Path
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
    [PathCompletions('.', 'Directory')]
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

  $GitCommitArguments, $Messages = (
    $Message ? (, $Message + $args) : $args
  ).Where({ $_ }).Where(
    { $_ -match $GIT_ARGUMENT },
    'Split'
  )

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    if ($Path -match $GIT_ARGUMENT -and -not $Messages) {
      $GitCommitArguments = , $Path + $GitCommitArguments
    }
    else {
      $Messages = , $Path + $Messages
    }

    $Path = ''
  }

  if ($AllowEmpty -and '--allow-empty' -notin $GitCommitArguments) {
    $GitCommitArguments += '--allow-empty'
  }

  if (-not $Messages) {
    if ('--allow-empty' -in $GitCommitArguments) {
      $Messages += 'No message.'
    }
    else {
      throw 'Missing commit message.'
    }
  }

  $GitParameters = @{
    Path  = $Path
    Throw = $Throw
  }
  if (-not $Staged) {
    Add-GitRepository @GitParameters -Throw
  }

  $GitCommitArguments = '-m', ($Messages -join ' ') + $GitCommitArguments
  $Commit = @{
    Verb = 'commit'
  }
  Invoke-GitRepository @Commit @GitParameters @GitCommitArguments
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
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $GitPushArguments = $args

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    $GitPushArguments = , $Path + $GitPushArguments
    $Path = ''
  }

  Get-GitRepository @PSBoundParameters -Throw

  $Push = @{
    Verb = 'push'
  }
  Invoke-GitRepository @Push @PSBoundParameters @GitPushArguments
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
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # The tree spec to which to revert, specified as '[HEAD]([~]|^)[n]'. If the tree spec is not specified, it defaults to HEAD. If only the number index is given, it defaults to '~' branching. If only the branching is given, the index defaults to 0 = HEAD.
    [string]$Tree,
    # Stop execution on Git error
    [switch]$Throw,
    # Remove --hard flag (no destructive reset)
    [switch]$Soft
  )

  $GitResetArguments = $args

  if ($Tree) {
    if (
      $Tree -match '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$' -and (
        -not $Matches.Step -or $Matches.Step -as [uint32]
      )
    ) {
      $Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }
    else {
      $GitResetArguments = , $Tree + $GitResetArguments
      $Tree = ''
    }
  }

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    if (
      -not $Tree -and $Path -match '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$' -and (
        -not $Matches.Step -or $Matches.Step -as [uint32]
      )
    ) {
      $Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }
    else {
      $GitResetArguments = , $Path + $GitResetArguments
    }

    $Path = ''
  }

  $GitParameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-GitRepository @GitParameters -Throw

  if ($Tree) {
    $GitResetArguments = , $Tree + $GitResetArguments
  }
  $GitResetArguments = , '--hard' + $GitResetArguments
  $Reset = @{
    Verb = 'reset'
  }
  Invoke-GitRepository @Reset @GitParameters @GitResetArguments
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
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $GitResetArguments = $args

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    $GitResetArguments = , $Path + $GitResetArguments
    $Path = ''
  }

  Reset-GitRepository @PSBoundParameters -Throw @GitResetArguments
  Get-GitRepository @PSBoundParameters
}
