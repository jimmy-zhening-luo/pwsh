using namespace System.Collections.Generic
using namespace Completer
using namespace Completer.PathCompleter

function Resolve-GitRepository {

  [CmdletBinding()]

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
    [string[]]$WorkingDirectory,

    [switch]$New

  )

  process {
    foreach ($Private:directory in $WorkingDirectory) {
      if ($New) {
        [hashtable]$Private:TestWorkingDirectory = @{
          Path = $WorkingDirectory
        }
        if (Test-RelativePath @Private:TestWorkingDirectory) {
          Write-Output ([string](Resolve-RelativePath @Private:TestWorkingDirectory))
        }
        else {
          $Private:TestWorkingDirectory.Location = $REPO_ROOT
          $Private:TestWorkingDirectory.New = $True

          if (Test-RelativePath @Private:TestWorkingDirectory) {
            Write-Output ([string](Resolve-RelativePath @Private:TestWorkingDirectory))
          }
        }
      }
      else {
        [hashtable]$Private:ResolveRepository = @{
          Path = $WorkingDirectory
        }
        [hashtable]$Private:TestRepository = @{
          Path           = $WorkingDirectory ? (Join-Path $WorkingDirectory .git) : '.git'
          RequireSubpath = $True
        }

        if (Test-RelativePath @Private:TestRepository) {
          Write-Output ([string](Resolve-RelativePath @Private:ResolveRepository))
        }
        else {
          $Private:TestRepository.Location = $Private:ResolveRepository.Location = $REPO_ROOT
          if (Test-RelativePath @Private:TestRepository) {
            Write-Output ([string](Resolve-RelativePath @Private:ResolveRepository))
          }
        }
      }
    }
  }
}

[string[]]$GIT_VERB = @(
  'switch'
  'merge'
  'diff'
  'stash'
  'tag'
  'config'
  'remote'
  'submodule'
  'fetch'
  'checkout'
  'branch'
  'rm'
  'mv'
  'ls-files'
  'ls-tree'
  'init'
  'status'
  'clone'
  'pull'
  'add'
  'commit'
  'push'
  'reset'
)
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

For every verb, if the Git command returns a non-zero exit code, the function will throw an error by default.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs
#>
function Invoke-GitRepository {

  [CmdletBinding()]

  param(

    [Parameter(
      Position = 0
    )]
    [StaticCompletions(
      'switch,merge,diff,stash,tag,config,remote,submodule,fetch,checkout,branch,rm,mv,ls-files,ls-tree,init,status,clone,pull,add,commit,push,reset',
      $null, $null
    )]
    # Git command to run.
    [string]$Verb,

    [Parameter(
      Position = 1
    )]
    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. For all verbs except 'clone', 'config', and 'init', the function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    # Additional arguments to pass to the git command.
    [string[]]$Argument,

    # When git command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    # Pass the -D flag as an argument to git
    [switch]$D,

    # Pass the -E flag as an argument to git
    [switch]$E,

    # Pass the -i flag as an argument to git
    [switch]$I,

    # Pass the -o flag as an argument to git
    [switch]$O,

    # Pass the -P flag as an argument to git
    [switch]$P,

    # Pass the -v flag as an argument to git
    [switch]$V

  )

  $Private:GitArgument = [List[string]]::new()

  if ($Verb) {
    if ($Verb -in $GIT_VERB) {
      if ($Verb -in $NEWABLE_GIT_VERB) {
        if ($WorkingDirectory -match $GIT_ARGUMENT) {
          $Private:GitArgument.Add($WorkingDirectory)
          $WorkingDirectory = ''
        }
      }
      else {
        if (
          $WorkingDirectory -and -not (
            $PWD | Resolve-GitRepository
          ) -and -not (
            $WorkingDirectory | Resolve-GitRepository
          ) -and (
            $Verb | Resolve-GitRepository
          )
        ) {
          $Private:GitArgument.Add($WorkingDirectory)
          $Verb, $WorkingDirectory = 'status', $Verb
        }
      }
    }
    else {
      if ($WorkingDirectory -or $Argument) {
        $Private:GitArgument.Add($Verb)
      }
      else {
        $WorkingDirectory = $Verb
      }

      $Verb = 'status'
    }
  }
  else {
    $Verb = 'status'
  }

  [hashtable]$Private:Resolve = @{
    WorkingDirectory = $WorkingDirectory
    New              = $Verb -in $NEWABLE_GIT_VERB
  }
  [string]$Private:Repository = Resolve-GitRepository @Private:Resolve

  if (-not $Private:Repository) {
    if ($WorkingDirectory) {
      $Private:GitArgument.Insert(0, $WorkingDirectory)

      $Private:Resolve.WorkingDirectory = $PWD.Path
      $Private:Repository = Resolve-GitRepository @Private:Resolve
    }

    if (-not $Private:Repository) {
      throw "Path '$WorkingDirectory' is not a Git repository"
    }
  }

  [string[]]$Private:GitCommand = @(
    '-c'
    'color.ui=always'
    '-C'
    $Private:Repository
    $Verb
  )

  if ($D) {
    $Private:GitArgument.Add('-D')
  }
  if ($E) {
    $Private:GitArgument.Add('-E')
  }
  if ($I) {
    $Private:GitArgument.Add('-i')
  }
  if ($O) {
    $Private:GitArgument.Add('-o')
  }
  if ($P) {
    $Private:GitArgument.Add('-P')
  }
  if ($Version) {
    $Private:GitArgument.Add('-v')
  }
  if ($Argument) {
    $Private:GitArgument.AddRange(
      [List[string]]$Argument
    )
  }

  & "$env:ProgramFiles\Git\cmd\git.exe" @Private:GitCommand @Private:GitArgument

  if ($LASTEXITCODE -ne 0) {
    [string]$Private:Exception = "git command error, execution returned exit code: $LASTEXITCODE"

    if ($NoThrow) {
      Write-Warning -Message $Private:Exception
    }
    else {
      throw $Private:Exception
    }
  }
}

<#
.SYNOPSIS
Use Git to get the status of a local repository.

.DESCRIPTION
This function is an alias for 'git status [argument]'.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-status
#>
function Measure-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  [hashtable]$Private:Status = @{
    Verb             = 'status'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Private:Status @args
}

<#
.SYNOPSIS
Use Git to clone a repository.

.DESCRIPTION
This function is an alias for 'git clone' and allows you to clone a repository into a specified path.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-clone
#>
function Import-GitRepository {

  param(

    # Remote repository URL or 'org/repo'
    [string]$Repository,

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to the directory into which the repository will be cloned. If not specified, defaults to the current location. The repository will be cloned into a subdirectory with the same name as the repository. If the path points to a container which does not exist, it will be created. If parent container creation fails, this function will throw an error. If Git encounters an error during cloning, this function will throw an error.
    [string]$WorkingDirectory,

    [Alias('ssh')]
    # Use git@github.com remote protocol instead of the default HTTPS
    [switch]$ForceSsh

  )

  [string[]]$Private:RepositoryPathSegments = $Repository -split '/' -notmatch [regex]'^\s*$'

  if (-not $Private:RepositoryPathSegments) {
    throw 'No repository name given.'
  }

  if ($Private:RepositoryPathSegments.Count -eq 1) {
    $Private:RepositoryPathSegments = , 'jimmy-zhening-luo' + $Private:RepositoryPathSegments
  }

  [string]$Private:Origin = (
    $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  ) + (
    $Private:RepositoryPathSegments -join '/'
  )

  $Private:CloneArgument = [List[string]]::new()
  $Private:CloneArgument.Add($Private:Origin)
  if ($args) {
    $Private:CloneArgument.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:Clone = @{
    Verb             = 'clone'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Private:Clone @Private:CloneArgument
}

<#
.SYNOPSIS
Use Git to pull changes from a repository.

.DESCRIPTION
This function is an alias for 'git pull [argument]'.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-pull
#>
function Get-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  [hashtable]$Private:Pull = @{
    Verb             = 'pull'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Private:Pull @args
}

<#
.SYNOPSIS
Use Git to pull changes for all repositories in the top level of %USERPROFILE%\code'.

.DESCRIPTION
This function runs 'git pull [argument]' in each child repository in %USERPROFILE%\code'.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildGitRepository {

  [hashtable]$Private:CodeDirectory = @{
    Path      = $REPO_ROOT
    Directory = $True
  }
  [string[]]$Private:Repositories = Get-ChildItem @Private:CodeDirectory |
    Select-Object -ExpandProperty FullName |
    Resolve-GitRepository

  foreach ($Private:Repository in $Private:Repositories) {
    Get-GitRepository -WorkingDirectory $Private:Repository @args
  }

  [ushort]$Private:Count = $Private:Repositories.Count

  return "`nPulled $Private:Count repositor" + ($Private:Count -eq 1 ? 'y' : 'ies')
}

<#
.SYNOPSIS
Use Git to stage all changes in a repository.

.DESCRIPTION
This function is an alias for 'git add [.]' and stages all changes in the repository.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-add
#>
function Add-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # File pattern of files to add, defaults to '.' (all)
    [string]$Name,

    # Equivalent to git add --renormalize flag
    [switch]$Renormalize

  )

  $Private:AddArgument = [List[string]]::new()

  if (-not $Name) {
    $Name = '.'
  }
  if ($Name -match $GIT_ARGUMENT) {
    $Private:AddArgument.Add($Name)
    $Name = ''
  }

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    if ($Name) {
      $Private:AddArgument.Insert(0, $WorkingDirectory)
    }
    else {
      $Name = $WorkingDirectory
    }

    $WorkingDirectory = ''
  }

  if ($Name) {
    $Private:AddArgument.Insert(0, $Name)
  }

  if ($args) {
    $Private:AddArgument.AddRange(
      [List[string]]$args
    )
  }

  if ($Renormalize -and '--renormalize' -notin $Private:AddArgument) {
    $Private:AddArgument.Add('--renormalize')
  }

  [hashtable]$Private:Add = @{
    Verb             = 'add'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Private:Add @Private:AddArgument
}

<#
.SYNOPSIS
Commit changes to a Git repository.

.DESCRIPTION
This function commits changes to a Git repository using the 'git commit' command.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-commit
#>
function Write-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # Commit message. It must be non-empty except on an empty commit, where it defaults to 'No message.'
    [string]$Message,

    # Do not add unstaged nor untracked files: only commit files that are already staged.
    [switch]$Staged,

    # Allow an empty commit, equivalent to git commit --allow-empty flag.
    [switch]$AllowEmpty

  )

  $Private:CommitArgument = [List[string]]::new()
  $Private:Messages = [List[string]]::new()

  [string[]]$Private:Argument, [string[]]$Private:MessageWord = (
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

  if ($Private:Argument) {
    $Private:CommitArgument.AddRange(
      [List[string]]$Private:Argument
    )
  }
  if ($Private:MessageWord) {
    $Private:Messages.AddRange(
      [List[string]]$Private:MessageWord
    )
  }

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    if ($WorkingDirectory -match $GIT_ARGUMENT -and $Private:Messages.Count -eq 0) {
      $Private:CommitArgument.Insert(0, $WorkingDirectory)
    }
    else {
      $Private:Messages.Insert(0, $WorkingDirectory)
    }

    $WorkingDirectory = ''
  }

  if ($AllowEmpty -and '--allow-empty' -notin $Private:CommitArgument) {
    $Private:CommitArgument.Add('--allow-empty')
  }

  if ($Private:Messages.Count -eq 0) {
    if ('--allow-empty' -in $Private:CommitArgument) {
      $Private:Messages.Add('No message.')
    }
    else {
      throw 'Missing commit message.'
    }
  }
  $Private:CommitArgument.InsertRange(
    0,
    [List[string]]@(
      '-m'
      $Private:Messages -join ' '
    )
  )

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  if (-not $Staged) {
    Add-GitRepository @Private:Repository
  }

  $Private:Repository.Verb = 'commit'
  Invoke-GitRepository @Private:Repository @Private:CommitArgument
}

<#
.SYNOPSIS
Use Git to push changes to a repository.

.DESCRIPTION
This function is an alias for 'git push'.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-push
#>
function Push-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  $Private:PushArgument = [List[string]]::new()

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    $Private:PushArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  if ($args) {
    $Private:PushArgument.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  Get-GitRepository @Private:Repository

  $Private:Repository.Verb = 'push'
  Invoke-GitRepository @Private:Repository @Private:PushArgument
}

[regex]$TREE_SPEC = '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$'

<#
.SYNOPSIS
Use Git to undo changes in a repository.

.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [HEAD]([~]|^)[n]'.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # The tree spec to which to revert, specified as '[HEAD]([~]|^)[n]'. If the tree spec is not specified, it defaults to HEAD. If only the number index is given, it defaults to '~' branching. If only the branching is given, the index defaults to 0 = HEAD.
    [string]$Tree,

    # Non-destructive reset, equivalent to running git reset without the --hard flag.
    [switch]$Soft

  )

  $Private:ResetArgument = [List[string]]::new()
  if ($args) {
    $Private:ResetArgument.AddRange(
      [List[string]]$args
    )
  }

  if ($Tree) {
    if (
      $Tree -match $TREE_SPEC -and (
        -not $Matches.Step -or $Matches.Step -as [uint]
      )
    ) {
      [string]$Private:Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Private:Branching + $Matches.Step
    }
    else {
      $Private:ResetArgument.Insert(0, $Tree)
      $Tree = ''
    }
  }

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    if (
      -not $Tree -and $WorkingDirectory -match $TREE_SPEC -and (
        -not $Matches.Step -or $Matches.Step -as [uint]
      )
    ) {
      [string]$Private:Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Private:Branching + $Matches.Step
    }
    else {
      $Private:ResetArgument.Insert(0, $WorkingDirectory)
    }

    $WorkingDirectory = ''
  }

  if ($Tree) {
    $Private:ResetArgument.Insert(0, $Tree)
  }
  $Private:ResetArgument.Insert(0, '--hard')

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  Add-GitRepository @Private:Repository

  $Private:Repository.Verb = 'reset'
  Invoke-GitRepository @Private:Repository @Private:ResetArgument
}

<#
.SYNOPSIS
Use Git to restore a repository to its previous state.

.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.

.COMPONENT
Code.Git

.LINK
https://git-scm.com/docs/git-reset
#>
function Restore-GitRepository {

  param(

    [PathLocationCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  $Private:ResetArgument = [List[string]]::new()

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    $Private:ResetArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  if ($args) {
    $Private:ResetArgument.AddRange(
      [List[string]]$args
    )
  }

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  Reset-GitRepository @Private:Repository @Private:ResetArgument

  Get-GitRepository @Private:Repository
}

New-Alias g Invoke-GitRepository
New-Alias gg Measure-GitRepository
New-Alias gitcl Import-GitRepository
New-Alias gpp Get-ChildGitRepository
New-Alias ga Add-GitRepository
New-Alias gs Push-GitRepository
New-Alias gr Reset-GitRepository
New-Alias grp Restore-GitRepository
