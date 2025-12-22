using namespace System.IO
using namespace System.Collections.Generic
using namespace Completer
using namespace Completer.PathCompleter

function Test-RelativePath {

  [OutputType([bool])]
  param(

    [string]$Path,

    [string]$Location,

    [switch]$File
  )

  $Path = [Canonicalizer]::Normalize($Path)
  $Location = [Canonicalizer]::Normalize($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      if (
        [Canonicalizer]::IsDescendantOf(
          $Path,
          $Location
        )
      ) {
        $Path = [Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif (
    [Canonicalizer]::IsHomeRooted($Path)
  ) {
    $Path = [Canonicalizer]::RemoveHomeRoot($Path)

    if ($Location) {
      $Path = Join-Path $HOME $Path

      if (
        [Canonicalizer]::IsDescendantOf(
          $Path,
          $Location
        )
      ) {
        $Path = [Path]::GetRelativePath(
          $Location,
          $Path
        )
      }
      else {
        return $False
      }
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  if (-not (Test-Path $Location -PathType Container)) {
    return $False
  }

  $FullLocation = (Resolve-Path $Location).Path
  $FullPath = Join-Path $FullLocation $Path
  [bool]$HasSubpath = $FullPath.Substring($FullLocation.Length) -notmatch [regex]'^\\*$'

  return $HasSubpath ? (
    Test-Path $FullPath -PathType (
      $File ? 'Leaf' : 'Container'
    )
  ) : (
    -not $File
  )
}

function Resolve-RelativePath {

  [OutputType([string])]
  param(

    [string]$Path,

    [string]$Location,

    [switch]$File
  )

  if (-not (Test-RelativePath @PSBoundParameters)) {
    throw "Invalid path '$Path'."
  }

  $Path = [Canonicalizer]::Normalize($Path)
  $Location = [Canonicalizer]::Normalize($Location)

  if ([Path]::IsPathRooted($Path)) {
    if ($Location) {
      $Path = [Path]::GetRelativePath(
        $Location,
        $Path
      )
    }
    else {
      $Location = [Path]::GetPathRoot($Path)
    }
  }
  elseif (
    [Canonicalizer]::IsHomeRooted($Path)
  ) {
    $Path = [Canonicalizer]::RemoveHomeRoot($Path)

    if ($Location) {
      $Path = [Path]::GetRelativePath(
        $Location,
        (Join-Path $HOME $Path)
      )
    }
    else {
      $Location = $HOME
    }
  }

  if (-not $Location) {
    $Location = $PWD.Path
  }

  return (Resolve-Path ($FullPath = Join-Path (Resolve-Path $Location).Path $Path) -Force).Path
}

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
    foreach ($directory in $WorkingDirectory) {
      if ($Newable) {
        if (-not $WorkingDirectory) {
          Write-Output -InputObject $PWD.Path
        }
        elseif (Test-Path $WorkingDirectory -PathType Container) {
          Write-Output -InputObject (
            Resolve-Path $WorkingDirectory
          ).Path
        }
        elseif (Test-RelativePath -Path $WorkingDirectory -Location $REPO_ROOT) {
          Write-Output -InputObject (
            Resolve-RelativePath -Path $WorkingDirectory -Location $REPO_ROOT
          )
        }
      }
      else {
        if (-not $WorkingDirectory) {
          if (Test-Path .git -PathType Container) {
            Write-Output -InputObject $PWD.Path
          }
        }
        elseif (
          Test-Path (
            Join-Path $WorkingDirectory .git
          ) -PathType Container
        ) {
          Write-Output -InputObject (
            Resolve-Path $WorkingDirectory
          ).Path
        }
        elseif (
          Test-RelativePath -Path (
            Join-Path $WorkingDirectory .git
          ) -Location $REPO_ROOT
        ) {
          Write-Output -InputObject (
            Resolve-RelativePath -Path $WorkingDirectory -Location $REPO_ROOT
          )
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
          $WorkingDirectory = ''
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
Code.Git

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

  Invoke-GitRepository -Verb status -WorkingDirectory $WorkingDirectory -Argument $args
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

  [string[]]$RepositoryPathSegments = $Repository -split '/' -notmatch [regex]'^\s*$'

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

  Invoke-GitRepository -Verb clone -WorkingDirectory $WorkingDirectory -Argument $CloneArgument
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

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory
  )

  Invoke-GitRepository -Verb pull -WorkingDirectory $WorkingDirectory -Argument $args
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

    [PathCompletions(
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

  $AddArgument = [List[string]]::new()

  if (-not $Name) {
    $Name = '.'
  }
  if ($Name -match $GIT_ARGUMENT) {
    $AddArgument.Add($Name)
    $Name = ''
  }

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and -not (
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    if ($Name) {
      $AddArgument.Insert(0, $WorkingDirectory)
    }
    else {
      $Name = $WorkingDirectory
    }

    $WorkingDirectory = ''
  }

  if ($Name) {
    $AddArgument.Insert(0, $Name)
  }

  if ($args) {
    $AddArgument.AddRange(
      [List[string]]$args
    )
  }

  if ($Renormalize -and '--renormalize' -notin $AddArgument) {
    $AddArgument.Add('--renormalize')
  }

  Invoke-GitRepository -Verb add -WorkingDirectory $WorkingDirectory -Argument $AddArgument
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

    $WorkingDirectory = ''
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

  Invoke-GitRepository -Verb commit -WorkingDirectory $WorkingDirectory -Argument $CommitArgument
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
    $WorkingDirectory = ''
  }

  if ($args) {
    $PushArgument.AddRange(
      [List[string]]$args
    )
  }

  Get-GitRepository -WorkingDirectory $WorkingDirectory

  Invoke-GitRepository -Verb push -WorkingDirectory $WorkingDirectory -Argument $PushArgument
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
      $Tree = ''
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

    $WorkingDirectory = ''
  }

  if ($Tree) {
    $ResetArgument.Insert(0, $Tree)
  }
  $ResetArgument.Insert(0, '--hard')

  Add-GitRepository -WorkingDirectory $WorkingDirectory

  Invoke-GitRepository -Verb reset -WorkingDirectory $WorkingDirectory -Argument $ResetArgument
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
    $WorkingDirectory = ''
  }

  if ($args) {
    $ResetArgument.AddRange(
      [List[string]]$args
    )
  }

  Reset-GitRepository -WorkingDirectory $WorkingDirectory @ResetArgument

  Get-GitRepository -WorkingDirectory $WorkingDirectory
}

New-Alias g Invoke-GitRepository
New-Alias gg Measure-GitRepository
New-Alias gitcl Import-GitRepository
New-Alias gpp Get-ChildGitRepository
New-Alias ga Add-GitRepository
New-Alias gs Push-GitRepository
New-Alias gr Reset-GitRepository
New-Alias grp Restore-GitRepository
