using namespace System.IO
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

    [switch]$Newable
  )

  process {
    foreach ($wd in $WorkingDirectory) {
      if ($Newable) {
        if (-not $wd) {
          Write-Output $PWD.Path
        }
        elseif (Test-Path $wd -PathType Container) {
          Write-Output (
            Resolve-Path $wd
          ).Path
        }
        elseif (
          -not [Path]::IsPathRooted(
            $wd
          ) -and (
            Test-Path (
              Join-Path $REPO_ROOT $wd
            ) -PathType Container
          )
        ) {
          Write-Output (
            Resolve-Path (
              Join-Path $REPO_ROOT $wd
            ) -Force
          ).Path
        }
      }
      else {
        if (-not $wd) {
          if (Test-Path .git -PathType Container) {
            Write-Output $PWD.Path
          }
        }
        elseif (
          Test-Path (
            Join-Path $wd .git
          ) -PathType Container
        ) {
          Write-Output (
            Resolve-Path $wd
          ).Path
        }
        elseif (
          -not [Path]::IsPathRooted(
            $wd
          ) -and (
            Test-Path (
              Join-Path $REPO_ROOT\$wd .git
            ) -PathType Container
          )
        ) {
          Write-Output (
            Resolve-Path (
              Join-Path $REPO_ROOT $wd
            ) -Force
          ).Path
        }
      }
    }
  }
}

function Test-NodePackageDirectory {
  [CmdletBinding()]
  [OutputType([bool])]
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [AllowEmptyString()]
    [string]$WorkingDirectory
  )

  if (-not $WorkingDirectory) {
    $WorkingDirectory = $PWD.Path
  }

  return Test-Path (
    Join-Path $WorkingDirectory package.json
  ) -PathType Leaf
}

function Resolve-NodePackageDirectory {
  [CmdletBinding()]
  [OutputType([string])]
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [AllowEmptyString()]
    [string]$WorkingDirectory
  )

  if (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory) {
    return $WorkingDirectory ? "--prefix=$((Resolve-Path $WorkingDirectory).Path)" : [string]::Empty
  }
  else {
    throw "Path '$WorkingDirectory' is not a Node package directory."
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
$GIT_ARGUMENT = '^(?>(?=.*[*=])(?>.+)|-(?>\w|(?>-\w[-\w]*\w)))$'

<#
.SYNOPSIS
Invoke a Git command in a local repository.

.DESCRIPTION
This function allows you to run a Git command in a local repository. If no command is specified, it defaults to 'git status'. If no path is specified, it defaults to the current location.

For every verb except for 'clone', 'config', and 'init', the function will throw an error if there is no Git repository at the specified path.

For every verb, if the Git command returns a non-zero exit code, the function will throw an error by default.

.COMPONENT
Code

.LINK
https://git-scm.com/docs
#>
function Invoke-Git {

  [CmdletBinding()]
  param(

    [Parameter(
      Position = 0
    )]
    [Completions(
      'switch,merge,diff,stash,tag,config,remote,submodule,fetch,checkout,branch,rm,mv,ls-files,ls-tree,init,status,clone,pull,add,commit,push,reset'
    )]
    # Git command to run.
    [string]$Verb,

    [Parameter(
      Position = 1
    )]
    [PathCompletions(
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

  $GitArgument = [List[string]]::new()

  if ($Verb) {
    if ($Verb -in $GIT_VERB) {
      if ($Verb -in $NEWABLE_GIT_VERB) {
        if ($WorkingDirectory -match $GIT_ARGUMENT) {
          $GitArgument.Add($WorkingDirectory)
          $WorkingDirectory = [string]::Empty
        }
      }
      else {
        if (
          $WorkingDirectory -and -not (
            Resolve-GitRepository -WorkingDirectory $PWD.Path
          ) -and -not (
            Resolve-GitRepository -WorkingDirectory $WorkingDirectory
          ) -and (
            Resolve-GitRepository -WorkingDirectory $Verb
          )
        ) {
          $GitArgument.Add($WorkingDirectory)
          $Verb, $WorkingDirectory = 'status', $Verb
        }
      }
    }
    else {
      if ($WorkingDirectory -or $Argument) {
        $GitArgument.Add($Verb)
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

  $Resolve = @{
    WorkingDirectory = $WorkingDirectory
    Newable          = $Verb -in $NEWABLE_GIT_VERB
  }
  $Repository = Resolve-GitRepository @Resolve

  if (-not $Repository) {
    if ($WorkingDirectory) {
      $GitArgument.Insert(0, $WorkingDirectory)

      $Resolve.WorkingDirectory = $PWD.Path
      $Repository = Resolve-GitRepository @Resolve
    }

    if (-not $Repository) {
      throw "Path '$WorkingDirectory' is not a Git repository"
    }
  }

  [string[]]$GitCommand = @(
    '-c'
    'color.ui=always'
    '-C'
    $Repository
    $Verb
  )

  if ($D) {
    $GitArgument.Add('-D')
  }
  if ($E) {
    $GitArgument.Add('-E')
  }
  if ($I) {
    $GitArgument.Add('-i')
  }
  if ($O) {
    $GitArgument.Add('-o')
  }
  if ($P) {
    $GitArgument.Add('-P')
  }
  if ($Version) {
    $GitArgument.Add('-v')
  }
  if ($Argument) {
    $GitArgument.AddRange(
      [List[string]]$Argument
    )
  }

  & "$env:ProgramFiles\Git\cmd\git.exe" @GitCommand @GitArgument

  if ($LASTEXITCODE -notin 0, 1) {
    $Exception = "git command error, execution returned exit code: $LASTEXITCODE"

    if ($NoThrow) {
      Write-Warning -Message "$Exception"
    }
    else {
      throw $Exception
    }
  }
}

<#
.SYNOPSIS
Use Git to get the status of a local repository.

.DESCRIPTION
This function is an alias for 'git status [argument]'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-status
#>
function Measure-GitRepository {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory
  )

  Invoke-Git -Verb status -WorkingDirectory $WorkingDirectory -Argument $args
}

<#
.SYNOPSIS
Use Git to clone a repository.

.DESCRIPTION
This function is an alias for 'git clone' and allows you to clone a repository into a specified path.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-clone
#>
function Import-GitRepository {

  param(

    # Remote repository URL or 'org/repo'
    [string]$Repository,

    [PathCompletions(
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

  [string[]]$RepositoryPathSegments = $Repository -split '/' -notmatch '^\s*$'

  if (-not $RepositoryPathSegments) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathSegments.Count -eq 1) {
    $RepositoryPathSegments = , 'jimmy-zhening-luo' + $RepositoryPathSegments
  }

  $Origin = (
    $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  ) + (
    $RepositoryPathSegments -join '/'
  )

  $CloneArgument = [List[string]]::new()
  $CloneArgument.Add($Origin)
  if ($args) {
    $CloneArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Git -Verb clone -WorkingDirectory $WorkingDirectory -Argument $CloneArgument
}

<#
.SYNOPSIS
Use Git to pull changes from a repository.

.DESCRIPTION
This function is an alias for 'git pull [argument]'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-pull
#>
function Get-GitRepository {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory
  )

  Invoke-Git -Verb pull -WorkingDirectory $WorkingDirectory -Argument $args
}

<#
.SYNOPSIS
Use Git to pull changes for all repositories in the top level of %USERPROFILE%\code'.

.DESCRIPTION
This function runs 'git pull [argument]' in each child repository in %USERPROFILE%\code'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildGitRepository {

  [CmdletBinding()]
  param()

  [string[]]$Repositories = Get-ChildItem -Path $REPO_ROOT -Directory |
    Select-Object -ExpandProperty FullName |
    Resolve-GitRepository

  foreach ($Repository in $Repositories) {
    Get-GitRepository -WorkingDirectory $Repository
  }

  $Count = $Repositories.Count

  return "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}

<#
.SYNOPSIS
Use Git to diff the current local working tree against the current local index.

.DESCRIPTION
This function is an alias for 'git diff [path]'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-diff
#>
function Compare-GitRepository {

  param(

    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    # File pattern of files to diff, defaults to '.' (all)
    [string]$Name,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory
  )

  $DiffArgument = [List[string]]::new()

  if ($Name) {
    $DiffArgument.Add($Name)
  }

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    $DiffArgument.Add($WorkingDirectory)
    $WorkingDirectory = [string]::Empty
  }

  if ($args) {
    $DiffArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Git -Verb diff -WorkingDirectory $WorkingDirectory -Argument $DiffArgument
}

<#
.SYNOPSIS
Use Git to stage all changes in a repository.

.DESCRIPTION
This function is an alias for 'git add [.]' and stages all changes in the repository.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-add
#>
function Add-GitRepository {

  param(

    [RelativePathCompletions(
      { return $PWD.Path }
    )]
    # File pattern of files to add, defaults to '.' (all)
    [string]$Name,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # Equivalent to git add --renormalize flag
    [switch]$Renormalize
  )

  if (-not $Name) {
    $Name = '.'
  }

  $AddArgument = [List[string]]::new()
  $AddArgument.Add($Name)

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    $AddArgument.Add($WorkingDirectory)
    $WorkingDirectory = [string]::Empty
  }

  if ($args) {
    $AddArgument.AddRange(
      [List[string]]$args
    )
  }

  if ($Renormalize -and '--renormalize' -notin $AddArgument) {
    $AddArgument.Add('--renormalize')
  }

  Invoke-Git -Verb add -WorkingDirectory $WorkingDirectory -Argument $AddArgument
}

<#
.SYNOPSIS
Commit changes to a Git repository.

.DESCRIPTION
This function commits changes to a Git repository using the 'git commit' command.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-commit
#>
function Write-GitRepository {

  param(

    [PathCompletions(
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

  $CommitArgument = [List[string]]::new()
  $Messages = [List[string]]::new()

  [string[]]$Argument, [string[]]$MessageWord = (
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

  if ($Argument) {
    $CommitArgument.AddRange(
      [List[string]]$Argument
    )
  }
  if ($MessageWord) {
    $Messages.AddRange(
      [List[string]]$MessageWord
    )
  }

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    if ($WorkingDirectory -match $GIT_ARGUMENT -and $Messages.Count -eq 0) {
      $CommitArgument.Insert(0, $WorkingDirectory)
    }
    else {
      $Messages.Insert(0, $WorkingDirectory)
    }

    $WorkingDirectory = [string]::Empty
  }

  if ($AllowEmpty -and '--allow-empty' -notin $CommitArgument) {
    $CommitArgument.Add('--allow-empty')
  }

  if ($Messages.Count -eq 0) {
    if ('--allow-empty' -in $CommitArgument) {
      $Messages.Add('No message.')
    }
    else {
      throw 'Missing commit message.'
    }
  }
  $CommitArgument.InsertRange(
    0,
    [List[string]]@(
      '-m'
      $Messages -join ' '
    )
  )

  if (-not $Staged) {
    Add-GitRepository -WorkingDirectory $WorkingDirectory
  }

  Invoke-Git -Verb commit -WorkingDirectory $WorkingDirectory -Argument $CommitArgument
}

<#
.SYNOPSIS
Use Git to push changes to a repository.

.DESCRIPTION
This function is an alias for 'git push'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-push
#>
function Push-GitRepository {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory
  )

  $PushArgument = [List[string]]::new()

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    $PushArgument.Add($WorkingDirectory)
    $WorkingDirectory = [string]::Empty
  }

  if ($args) {
    $PushArgument.AddRange(
      [List[string]]$args
    )
  }

  Get-GitRepository -WorkingDirectory $WorkingDirectory

  Invoke-Git -Verb push -WorkingDirectory $WorkingDirectory -Argument $PushArgument
}

$TREE_SPEC = '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$'

<#
.SYNOPSIS
Use Git to undo changes in a repository.

.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [HEAD]([~]|^)[n]'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-GitRepository {

  param(

    [PathCompletions(
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

  $ResetArgument = [List[string]]::new()
  if ($args) {
    $ResetArgument.AddRange(
      [List[string]]$args
    )
  }

  if ($Tree) {
    if (
      $Tree -match $TREE_SPEC -and (
        -not $Matches.Step -or $Matches.Step -as [int]
      )
    ) {
      [string]$Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }
    else {
      $ResetArgument.Insert(0, $Tree)
      $Tree = [string]::Empty
    }
  }

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    if (
      -not $Tree -and $WorkingDirectory -match $TREE_SPEC -and (
        -not $Matches.Step -or $Matches.Step -as [int]
      )
    ) {
      [string]$Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = "HEAD$Branching$($Matches.Step)"
    }
    else {
      $ResetArgument.Insert(0, $WorkingDirectory)
    }

    $WorkingDirectory = [string]::Empty
  }

  if ($Tree) {
    $ResetArgument.Insert(0, $Tree)
  }
  $ResetArgument.Insert(0, '--hard')

  Add-GitRepository -WorkingDirectory $WorkingDirectory

  Invoke-Git -Verb reset -WorkingDirectory $WorkingDirectory -Argument $ResetArgument
}

<#
.SYNOPSIS
Use Git to restore a repository to its previous state.

.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.

.COMPONENT
Code

.LINK
https://git-scm.com/docs/git-reset
#>
function Restore-GitRepository {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory
  )

  $ResetArgument = [List[string]]::new()

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    $ResetArgument.Add($WorkingDirectory)
    $WorkingDirectory = [string]::Empty
  }

  if ($args) {
    $ResetArgument.AddRange(
      [List[string]]$args
    )
  }

  Reset-GitRepository -WorkingDirectory $WorkingDirectory @ResetArgument

  Get-GitRepository -WorkingDirectory $WorkingDirectory
}

[string[]]$NODE_VERB = @(
  'access'
  'adduser'
  'audit'
  'bugs'
  'cache'
  'ci'
  'completion'
  'config'
  'dedupe'
  'deprecate'
  'diff'
  'dist-tag'
  'docs'
  'doctor'
  'edit'
  'exec'
  'explain'
  'explore'
  'find-dupes'
  'fund'
  'help'
  'help-search'
  'init'
  'install'
  'install-ci-test'
  'install-test'
  'link'
  'login'
  'logout'
  'ls'
  'org'
  'outdated'
  'owner'
  'pack'
  'ping'
  'pkg'
  'prefix'
  'profile'
  'prune'
  'publish'
  'query'
  'rebuild'
  'repo'
  'restart'
  'root'
  'run'
  'sbom'
  'search'
  'shrinkwrap'
  'star'
  'stars'
  'start'
  'stop'
  'team'
  'test'
  'token'
  'undeprecate'
  'uninstall'
  'unpublish'
  'unstar'
  'update'
  'version'
  'view'
  'whoami'
)

$NODE_ALIAS = @{
  issues  = 'bugs'
  c       = 'config'
  ddp     = 'dedupe'
  home    = 'docs'
  why     = 'explain'
  create  = 'init'
  add     = 'install'
  i       = 'install'
  in      = 'install'
  ln      = 'link'
  cit     = 'install-ci-test'
  it      = 'install-test'
  list    = 'ls'
  author  = 'owner'
  rb      = 'rebuild'
  find    = 'search'
  s       = 'search'
  se      = 'search'
  t       = 'test'
  unlink  = 'uninstall'
  remove  = 'uninstall'
  rm      = 'uninstall'
  r       = 'uninstall'
  un      = 'uninstall'
  up      = 'update'
  upgrade = 'update'
  info    = 'view'
  show    = 'view'
  v       = 'view'
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run a command in a Node package.

.DESCRIPTION
This function runs an npm command in a specified Node package directory, or the current directory if no path is specified.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands

.LINK
https://docs.npmjs.com/cli/commands/npm
#>
function Invoke-Npm {

  [CmdletBinding()]
  param(

    [Parameter(
      Position = 0
    )]
    [Completions(
      'pkg,i,it,cit,rm,access,adduser,audit,bugs,cache,ci,completion,config,dedupe,deprecate,diff,dist-tag,docs,doctor,edit,exec,explain,explore,find-dupes,fund,help,help-search,init,install,install-ci-test,install-test,link,login,logout,ls,org,outdated,owner,pack,ping,prefix,profile,prune,publish,query,rebuild,repo,restart,root,run,sbom,search,shrinkwrap,star,stars,start,stop,team,test,token,undeprecate,uninstall,unpublish,unstar,update,version,view,whoami'
    )]
    # npm command verb
    [string]$Command,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments
    )]
    # Additional arguments to pass to npm
    [string[]]$Argument,

    # When npm command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    # Show npm version if no command is specified. Otherwise, pass the -v flag.
    [switch]$Version,

    # Pass the -D flag as an argument to npm
    [switch]$D,

    # Pass the -E flag as an argument to npm
    [switch]$E,

    # Pass the -i flag as an argument to npm
    [switch]$I,

    # Pass the -o flag as an argument to npm
    [switch]$O,

    # Pass the -P flag as an argument to npm
    [switch]$P,

    [Parameter(DontShow)][switch]$z
  )

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add('--color=always')

  $NodeCommand = [List[string]]::new()
  if ($Argument) {
    $NodeCommand.AddRange(
      [List[string]]$Argument
    )
  }

  if ($WorkingDirectory.Length -ne 0) {
    if (
      $WorkingDirectory.StartsWith([char]'-') -or -not (
        Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory
      )
    ) {
      $NodeCommand.Add($WorkingDirectory)
      $WorkingDirectory = [string]::Empty
    }
    else {
      $PackagePrefix = Resolve-NodePackageDirectory -WorkingDirectory $WorkingDirectory

      if ($PackagePrefix) {
        $NodeArgument.Add($PackagePrefix)
      }
    }
  }

  if ($Command.Length -ne 0 -and $Command.StartsWith('-') -or $Command -notin $NODE_VERB -and -not $NODE_ALIAS.ContainsKey($Command)) {
    [string]$DeferredVerb = $NodeCommand.Count -eq 0 ? [string]::Empty : $NodeCommand.Find(
      {
        $args[0] -in $NODE_VERB
      }
    )

    if ($DeferredVerb) {
      $NodeCommand.Remove($DeferredVerb) | Out-Null
    }

    $NodeCommand.Insert(0, $Command)
    $Command = $DeferredVerb
  }

  if ($Command) {
    $NodeArgument.Add($Command.ToLowerInvariant())
    if ($D) {
      $NodeCommand.Add('-D')
    }
    if ($E) {
      $NodeCommand.Add('-E')
    }
    if ($I) {
      $NodeCommand.Add('-i')
    }
    if ($O) {
      $NodeCommand.Add('-o')
    }
    if ($P) {
      $NodeCommand.Add('-P')
    }
    if ($Version) {
      $NodeCommand.Add('-v')
    }
  }
  else {
    if ($Version) {
      $NodeArgument.Add('-v')
    }
  }

  if ($NodeCommand.Count -ne 0) {
    $NodeArgument.AddRange(
      $NodeCommand
    )
  }

  & npm.ps1 @NodeArgument

  if ($LASTEXITCODE -notin 0, 1) {
    $Exception = "npm command error, execution returned exit code: $LASTEXITCODE"

    if ($NoThrow) {
      Write-Warning -Message "$Exception"
    }
    else {
      throw $Exception
    }
  }
}

<#
.SYNOPSIS
Run Node.

.DESCRIPTION
This function is an alias shim for 'node [args]'.

.COMPONENT
Code

.LINK
https://nodejs.org/api/cli.html
#>
function Invoke-Node {

  if ($args) {
    & node.exe @args

    if ($LASTEXITCODE -notin 0, 1) {
      throw "Node.exe error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
}

<#
.SYNOPSIS
Use 'npx' to run a command from a local or remote npm module.

.DESCRIPTION
This function is an alias shim for 'npx [args]'.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands/npx
#>
function Invoke-NodeExecutable {

  if ($args) {
    & npx.ps1 @args

    if ($LASTEXITCODE -notin 0, 1) {
      throw "npx error, execution stopped with exit code: $LASTEXITCODE"
    }
  }
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to clear the global Node module cache.

.DESCRIPTION
This function is an alias for 'npm cache clean --force'.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {

  $NodeArgument = [List[string]]::new(
    [List[string]]@(
      'clean'
      '--force'
    )
  )
  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Npm -Command cache -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node package.

.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $WorkingDirectory]'.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-NodeModule {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  $NodeArgument = [List[string]]::new()

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = [string]::Empty
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Npm -Command outdated -WorkingDirectory $WorkingDirectory -NoThrow -Argument $NodeArgument
}

enum NodePackageNamedVersion {
  patch
  minor
  major
  prerelease
  preminor
  premajor
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to increment the package version of the current Node package.

.DESCRIPTION
This function is an alias for 'npm version [--prefix $WorkingDirectory] [version=patch]'.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands/npm-version
#>
function Step-NodePackageVersion {

  param(

    # New package version, default 'patch'
    [Completions(
      'patch,minor,major,prerelease,preminor,premajor'
    )]
    [string]$Version,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  $Version = switch ($Version) {
    [string]::Empty {
      [NodePackageNamedVersion]::patch
      break
    }
    {
      $null -ne [NodePackageNamedVersion]::$Version
    } {
      [NodePackageNamedVersion]::$Version
      break
    }
    {
      $Version.StartsWith(
        [char]'v',
        [StringComparison]::OrdinalIgnoreCase
      )
    } {
      $Version = $Version.Substring(1)
    }
    default {
      $Semver = $null
      if ([semver]::TryParse($Version, $Semver)) {
        $Semver.ToString()
      }
      else {
        throw 'Provided version was neither a well-known version nor parseable as a semantic version.'
      }
    }
  }

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add(
    $Version.ToLowerInvariant()
  )

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = [string]::Empty
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Npm -Command version -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm run [script] [--prefix $WorkingDirectory] [--args]'.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands/npm-run
#>
function Invoke-NodePackageScript {

  param(

    # Name of the npm script to run
    [string]$Script,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  if (-not $Script) {
    throw 'Script name is required.'
  }

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add($Script)

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = [string]::Empty
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Npm -Command run -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run the 'test' script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm test [--prefix $WorkingDirectory] [--args]'.

.COMPONENT
Code

.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {

  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(DontShow)][switch]$z
  )

  $NodeArgument = [List[string]]::new()

  if ($WorkingDirectory) {
    if (-not (Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = [string]::Empty
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Npm -Command test -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

New-Alias g Invoke-Git
New-Alias gg Measure-GitRepository
New-Alias gitcl Import-GitRepository
New-Alias gpp Get-ChildGitRepository
New-Alias gd Compare-GitRepository
New-Alias ga Add-GitRepository
New-Alias gs Push-GitRepository
New-Alias gr Reset-GitRepository
New-Alias grp Restore-GitRepository
New-Alias n Invoke-Npm
New-Alias no Invoke-Node
New-Alias nx Invoke-NodeExecutable
New-Alias ncc Clear-NodeModuleCache
New-Alias npo Compare-NodeModule
New-Alias nu Step-NodePackageVersion
New-Alias nr Invoke-NodePackageScript
New-Alias nt Test-NodePackage
