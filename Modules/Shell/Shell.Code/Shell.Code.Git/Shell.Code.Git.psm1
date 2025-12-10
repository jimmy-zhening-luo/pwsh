using namespace System.Collections.Generic

function Resolve-GitRepository {
  [OutputType([string[]])]
  param(
    [Parameter(
      Mandatory,
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [string[]]$Path,
    [switch]$New
  )
  begin {
    [string]$Private:CODE_PATH = "$HOME\code"
    $Private:Repositories = [List[string]]::new()
  }
  process {
    [hashtable]$Private:RepoPath = @{
      Path = $Path
    }
    [string]$Private:Repository = ''

    if ($New) {
      if (Test-Item @RepoPath) {
        $Repository = Resolve-Item @RepoPath
      }
      else {
        $RepoPath.Location = $CODE_PATH
        $RepoPath.New = $True

        if (Test-Item @RepoPath) {
          $Repository = Resolve-Item @RepoPath
        }
      }
    }
    else {
      [hashtable]$Private:RepoGitPath = @{
        Path           = $Path ? (Join-Path $Path .git) : '.git'
        RequireSubpath = $True
      }

      if (Test-Item @RepoGitPath) {
        $Repository = Resolve-Item @RepoPath
      }
      else {
        $RepoGitPath.Location = $RepoPath.Location = $CODE_PATH

        if (Test-Item @RepoGitPath) {
          $Repository = Resolve-Item @RepoPath
        }
      }
    }

    if ($Repository) {
      $Repositories.Add([string]$Repository)
    }
  }
  end {
    return [string[]]$Repositories.ToArray()
  }
}

[string]$GIT_VERB_COMPLETION = (
  'switch,merge,diff,stash,tag,config,remote,submodule,fetch,checkout,branch,rm,mv,ls-files,ls-tree,init,status,clone,pull,add,commit,push,reset'
)
[string[]]$GIT_VERB = $GIT_VERB_COMPLETION -split ','
[string[]]$NEWABLE_GIT_VERB = @(
  'clone'
  'config'
  'init'
)
[regex]$GIT_ARGUMENT = '^(?>(?=.*[*=])(?>.+)|-(?>\w|(?>-\w[-\w]*\w)))$'

<#
.SYNOPSIS
Invoke a Git command in a local repository.
.DESCRIPTION
This function allows you to run a Git command in a local repository. If no command is specified, it defaults to 'git status'. If no path is specified, it defaults to the current location.

For every verb except for 'clone', 'config', and 'init', the function will throw an error if there is no Git repository at the specified path.
.LINK
https://git-scm.com/docs
#>
New-Alias g Invoke-GitRepository
function Invoke-GitRepository {
  param(
    [GenericCompletions(
      'switch,merge,diff,stash,tag,config,remote,submodule,fetch,checkout,branch,rm,mv,ls-files,ls-tree,init,status,clone,pull,add,commit,push,reset'
    )]
    # Git verb (command) to run
    [string]$Verb,
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $Private:GitArguments = [List[string]]::new()

  if ($args) {
    $GitArguments.AddRange([List[string]]$args)
  }

  if ($Verb) {
    if ($Verb -in $GIT_VERB) {
      if ($Verb -in $NEWABLE_GIT_VERB) {
        if ($Path -match $GIT_ARGUMENT) {
          $GitArguments.Insert(0, $Path)
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
          $GitArguments.Insert(0, $Path)
          $Verb, $Path = 'status', $Verb
        }
      }
    }
    else {
      if ($Path -or $GitArguments) {
        $GitArguments.Insert(0, $Verb)
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

  [hashtable]$Private:Resolve = @{
    Path = $Path
    New  = $Verb -in $NEWABLE_GIT_VERB
  }
  [string]$Private:Repository = Resolve-GitRepository @Resolve

  if (-not $Repository) {
    if ($Path) {
      $GitArguments.Insert(0, $Path)

      $Resolve.Path = $PWD
      $Repository = Resolve-GitRepository @Resolve
    }

    if (-not $Repository) {
      throw "Path '$Path' is not a Git repository"
    }
  }

  [string[]]$GitCommandManifest = @(
    '-c'
    'color.ui=always'
    '-C'
    $Repository
    $Verb
  )
  $GitArguments.InsertRange(0, [List[string]]$GitCommandManifest)

  if ($Throw) {
    & git.exe @GitArguments 2>&1 |
      Tee-Object -Variable GitResult

    if ($GitResult -match [regex]'^fatal:') {
      throw 'Git command error, execution stopped.'
    }
  }
  else {
    & git.exe @GitArguments
  }
}

New-Alias gg Measure-GitRepository
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

  [hashtable]$Private:Status = @{
    Verb = 'status'
  }
  Invoke-GitRepository @Status @PSBoundParameters @args
}

New-Alias gitcl Import-GitRepository
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

  [string[]]$Private:RepositoryPathParts = $Repository -split '/' -notmatch '^\s*$'

  if (-not $RepositoryPathParts) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathParts.Count -eq 1) {
    $RepositoryPathParts = , 'jimmy-zhening-luo' + $RepositoryPathParts
  }

  [string]$Private:Origin = (
    $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  ) + (
    $RepositoryPathParts -join '/'
  )

  $Private:GitCommandArguments = [List[string]]::new()
  $GitCommandArguments.Add($Origin)
  if ($args) {
    $GitCommandArguments.AddRange([List[string]]$args)
  }

  [hashtable]$Private:Clone = @{
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

  [hashtable]$Private:Pull = @{
    Verb = 'pull'
  }
  Invoke-GitRepository @Pull @PSBoundParameters @args
}

New-Alias gpp Get-ChildGitRepository
<#
.SYNOPSIS
Use Git to pull changes for all repositories in the top level of %USERPROFILE%\code'.
.DESCRIPTION
This function runs 'git pull [arguments]' in each child repository in %USERPROFILE%\code'.
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildGitRepository {
  [hashtable]$Private:CodeDirectory = @{
    Path      = "$HOME\code"
    Directory = $True
  }
  [string[]]$Private:Repositories = Get-ChildItem @CodeDirectory |
    Select-Object -ExpandProperty FullName |
    Resolve-GitRepository
  [UInt16]$Private:Count = $Repositories.Count

  foreach ($Private:Repository in $Repositories) {
    Get-GitRepository -Path $Repository @args
  }

  return "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}

New-Alias ga Add-GitRepository
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
    [string]$Name,
    # Stop execution on Git error
    [switch]$Throw,
    # Include '--renormalize' flag
    [switch]$Renormalize
  )

  $Private:GitCommandArguments = [List[string]]::new()
  if ($args) {
    $GitCommandArguments.AddRange([List[string]]$args)
  }

  if (-not $Name) {
    $Name = '.'
  }

  if ($Name -match $GIT_ARGUMENT) {
    $GitCommandArguments.Insert(0, $Name)
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
      $GitCommandArguments.Insert(0, $Path)
    }
    else {
      $Name = $Path
    }

    $Path = ''
  }

  if ($Name) {
    $GitCommandArguments.Insert(0, $Name)
  }

  if ($Renormalize -and '--renormalize' -notin $GitCommandArguments) {
    $GitCommandArguments.Add('--renormalize')
  }

  [hashtable]$Private:Add = @{
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

  [string[]]$Private:Arguments, [string[]]$Private:MessageWords = (
    $Message ? (, $Message + $args) : $args
  ).Where(
    {
      $PSItem
    }
  ).Where(
    {
      $PSItem -match $GIT_ARGUMENT
    },
    'Split'
  )

  $Private:GitCommitArguments = [List[string]]::new()
  if ($Arguments) {
    $GitCommitArguments.AddRange([List[string]]$Arguments)
  }

  $Private:Messages = [List[string]]::new()
  if ($MessageWords) {
    $Messages.AddRange([List[string]]$MessageWords)
  }

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    if ($Path -match $GIT_ARGUMENT -and $Messages.Count -eq 0) {
      $GitCommitArguments.Insert(0, $Path)
    }
    else {
      $Messages.Insert(0, $Path)
    }

    $Path = ''
  }

  if ($AllowEmpty -and '--allow-empty' -notin $GitCommitArguments) {
    $GitCommitArguments.Add('--allow-empty')
  }

  if ($Messages.Count -eq 0) {
    if ('--allow-empty' -in $GitCommitArguments) {
      $Messages.Add('No message.')
    }
    else {
      throw 'Missing commit message.'
    }
  }

  [hashtable]$Private:GitParameters = @{
    Path  = $Path
    Throw = $Throw
  }
  if (-not $Staged) {
    Add-GitRepository @GitParameters -Throw
  }

  [string[]]$MessageArguments = @(
    '-m'
    $Messages -join ' '
  )
  $GitCommitArguments.InsertRange(0, [List[string]]$MessageArguments)

  $Private:Commit = @{
    Verb = 'commit'
  }
  Invoke-GitRepository @Commit @GitParameters @GitCommitArguments
}

New-Alias gs Push-GitRepository
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

  $Private:GitPushArguments = [List[string]]::new()
  if ($args) {
    $GitPushArguments.AddRange([List[string]]$args)
  }

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    $GitPushArguments.Insert(0, $Path)
    $PSBoundParameters.Path = ''
  }

  Get-GitRepository @PSBoundParameters -Throw

  [hashtable]$Private:Push = @{
    Verb = 'push'
  }
  Invoke-GitRepository @Push @PSBoundParameters @GitPushArguments
}

New-Alias gr Reset-GitRepository
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

  $Private:GitResetArguments = [List[string]]::new()
  if ($args) {
    $GitResetArguments.AddRange([List[string]]$args)
  }

  [regex]$Private:TREE_SPEC = '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$'

  if ($Tree) {
    if (
      $Tree -match $TREE_SPEC -and (
        -not $Matches.Step -or $Matches.Step -as [uint32]
      )
    ) {
      [string]$Private:Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }
    else {
      $GitResetArguments.Insert(0, $Tree)
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
      -not $Tree -and $Path -match $TREE_SPEC -and (
        -not $Matches.Step -or $Matches.Step -as [uint32]
      )
    ) {
      [string]$Private:Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }
    else {
      $GitResetArguments.Insert(0, $Path)
    }

    $Path = ''
  }

  [hashtable]$Private:GitParameters = @{
    Path  = $Path
    Throw = $Throw
  }
  Add-GitRepository @GitParameters -Throw

  if ($Tree) {
    $GitResetArguments.Insert(0, $Tree)
  }
  $GitResetArguments.Insert(0, '--hard')

  [hashtable]$Private:Reset = @{
    Verb = 'reset'
  }
  Invoke-GitRepository @Reset @GitParameters @GitResetArguments
}

New-Alias grp Restore-GitRepository
<#
.SYNOPSIS
Use Git to restore a repository to its previous state.
.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.
.LINK
https://git-scm.com/docs/git-reset
#>
function Restore-GitRepository {
  param(
    [PathCompletions('.', 'Directory')]
    # Local repository path
    [string]$Path,
    # Stop execution on Git error
    [switch]$Throw
  )

  $Private:GitResetArguments = [List[string]]::new()
  if ($args) {
    $GitResetArguments.AddRange([List[string]]$args)
  }

  if (
    $Path -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $Path | Resolve-GitRepository
    )
  ) {
    $GitResetArguments.Insert(0, $Path)
    $PSBoundParameters.Path = ''
  }

  Reset-GitRepository @PSBoundParameters -Throw @GitResetArguments
  Get-GitRepository @PSBoundParameters
}
