using namespace System.Collections.Generic

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
    [string]$WorkingDirectory,

    [switch]$New

  )

  begin {
    [string]$Private:CODE_PATH = "$HOME\code"

    $Private:WorkingDirectories = [List[string]]::new()
  }

  process {
    if ($New) {
      [hashtable]$Private:TestWorkingDirectory = @{
        Path = $WorkingDirectory
      }
      if (Test-Item @TestWorkingDirectory) {
        $WorkingDirectories.Add([string](Resolve-Item @TestWorkingDirectory))
      }
      else {
        $TestWorkingDirectory.Location = $CODE_PATH
        $TestWorkingDirectory.New = $True

        if (Test-Item @TestWorkingDirectory) {
          $WorkingDirectories.Add([string](Resolve-Item @TestWorkingDirectory))
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

      if (Test-Item @TestRepository) {
        $WorkingDirectories.Add([string](Resolve-Item @ResolveRepository))
      }
      else {
        $TestRepository.Location = $ResolveRepository.Location = $CODE_PATH

        if (Test-Item @TestRepository) {
          $WorkingDirectories.Add([string](Resolve-Item @ResolveRepository))
        }
      }
    }
  }

  end {
    return [string[]]$WorkingDirectories.ToArray()
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

For every verb, if the Git command returns a non-zero exit code, the function will throw an error by default.

.LINK
https://git-scm.com/docs
#>
New-Alias g Invoke-GitRepository
function Invoke-GitRepository {

  param(

    [GenericCompletions(
      'switch,merge,diff,stash,tag,config,remote,submodule,fetch,checkout,branch,rm,mv,ls-files,ls-tree,init,status,clone,pull,add,commit,push,reset'
    )]
    # Git command to run.
    [string]$Verb,

    [PathCompletions('.', 'Directory')]
    # Path to local repository. If not specified, defaults to the current location. For all verbs except 'clone', 'config', and 'init', the function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  $Private:GitArguments = [List[string]]::new(
    [List[string]]$args
  )

  if ($Verb) {
    if ($Verb -in $GIT_VERB) {
      if ($Verb -in $NEWABLE_GIT_VERB) {
        if ($WorkingDirectory -match $GIT_ARGUMENT) {
          $GitArguments.Insert(0, $WorkingDirectory)
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
          $GitArguments.Insert(0, $WorkingDirectory)
          $Verb, $WorkingDirectory = 'status', $Verb
        }
      }
    }
    else {
      if ($WorkingDirectory -or $GitArguments) {
        $GitArguments.Insert(0, $Verb)
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
  [string]$Private:Repository = Resolve-GitRepository @Resolve

  if (-not $Repository) {
    if ($WorkingDirectory) {
      $GitArguments.Insert(0, $WorkingDirectory)

      $Resolve.WorkingDirectory = $PWD
      $Repository = Resolve-GitRepository @Resolve
    }

    if (-not $Repository) {
      throw "Path '$WorkingDirectory' is not a Git repository"
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

  & git.exe @GitArguments

  if ($LASTEXITCODE -ne 0) {
    throw "git command error, execution returned exit code: $LASTEXITCODE"
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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  [hashtable]$Private:Status = @{
    Verb             = 'status'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Status @args
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
    # Path to the directory into which the repository will be cloned. If not specified, defaults to the current location. The repository will be cloned into a subdirectory with the same name as the repository. If the path points to a container which does not exist, it will be created. If parent container creation fails, this function will throw an error. If Git encounters an error during cloning, this function will throw an error.
    [string]$WorkingDirectory,

    [Alias('ssh')]
    # Use git@github.com remote protocol instead of the default HTTPS
    [switch]$ForceSsh

  )

  [string[]]$Private:RepositoryPathSegments = $Repository -split '/' -notmatch [regex]'^\s*$'

  if (-not $RepositoryPathSegments) {
    throw 'No repository name given.'
  }

  if ($RepositoryPathSegments.Count -eq 1) {
    $RepositoryPathSegments = , 'jimmy-zhening-luo' + $RepositoryPathSegments
  }

  [string]$Private:Origin = (
    $ForceSsh ? 'git@github.com:' : 'https://github.com/'
  ) + (
    $RepositoryPathSegments -join '/'
  )

  $Private:CloneArguments = [List[string]]::new()
  $CloneArguments.Add($Origin)

  if ($args) {
    $CloneArguments.AddRange([List[string]]$args)
  }

  [hashtable]$Private:Clone = @{
    Verb             = 'clone'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Clone @CloneArguments
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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  [hashtable]$Private:Pull = @{
    Verb             = 'pull'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Pull @args
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

  foreach ($Private:Repository in $Repositories) {
    Get-GitRepository -WorkingDirectory $Repository @args
  }

  [UInt16]$Private:Count = $Repositories.Count

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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # File pattern of files to add, defaults to '.' (all)
    [string]$Name,

    # Equivalent to git add --renormalize flag
    [switch]$Renormalize

  )

  $Private:AddArguments = [List[string]]::new(
    [List[string]]$args
  )

  if (-not $Name) {
    $Name = '.'
  }

  if ($Name -match $GIT_ARGUMENT) {
    $AddArguments.Insert(0, $Name)
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
      $AddArguments.Insert(0, $WorkingDirectory)
    }
    else {
      $Name = $WorkingDirectory
    }

    $WorkingDirectory = ''
  }

  if ($Name) {
    $AddArguments.Insert(0, $Name)
  }

  if ($Renormalize -and '--renormalize' -notin $AddArguments) {
    $AddArguments.Add('--renormalize')
  }

  [hashtable]$Private:Add = @{
    Verb             = 'add'
    WorkingDirectory = $WorkingDirectory
  }
  Invoke-GitRepository @Add @AddArguments
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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # Commit message. It must be non-empty except on an empty commit, where it defaults to 'No message.'
    [string]$Message,

    # Do not add unstaged nor untracked files: only commit files that are already staged.
    [switch]$Staged,

    # Allow an empty commit, equivalent to git commit --allow-empty flag.
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

  $Private:CommitArguments = [List[string]]::new([List[string]]$Arguments)
  $Private:Messages = [List[string]]::new([List[string]]$MessageWords)

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    if ($WorkingDirectory -match $GIT_ARGUMENT -and $Messages.Count -eq 0) {
      $CommitArguments.Insert(0, $WorkingDirectory)
    }
    else {
      $Messages.Insert(0, $WorkingDirectory)
    }

    $WorkingDirectory = ''
  }

  if ($AllowEmpty -and '--allow-empty' -notin $CommitArguments) {
    $CommitArguments.Add('--allow-empty')
  }

  if ($Messages.Count -eq 0) {
    if ('--allow-empty' -in $CommitArguments) {
      $Messages.Add('No message.')
    }
    else {
      throw 'Missing commit message.'
    }
  }
  $CommitArguments.InsertRange(
    0,
    [List[string]]@(
      '-m'
      $Messages -join ' '
    )
  )

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }  
  if (-not $Staged) {
    Add-GitRepository @Repository
  }

  $Repository.Verb = 'commit'
  Invoke-GitRepository @Repository @CommitArguments
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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  $Private:PushArguments = [List[string]]::new()

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    $PushArguments.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  $PushArguments.AddRange([List[string]]$args)

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  Get-GitRepository @Repository

  $Repository.Verb = 'push'
  Invoke-GitRepository @Repository @PushArguments
}

[regex]$TREE_SPEC = '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$'

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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    # The tree spec to which to revert, specified as '[HEAD]([~]|^)[n]'. If the tree spec is not specified, it defaults to HEAD. If only the number index is given, it defaults to '~' branching. If only the branching is given, the index defaults to 0 = HEAD.
    [string]$Tree,

    # Non-destructive reset, equivalent to running git reset without the --hard flag.
    [switch]$Soft

  )

  $Private:ResetArguments = [List[string]]::new(
    [List[string]]$args
  )

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
      $ResetArguments.Insert(0, $Tree)
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
        -not $Matches.Step -or $Matches.Step -as [uint32]
      )
    ) {
      [string]$Private:Branching = $Matches.Branching ? $Matches.Branching : '~'
      $Tree = 'HEAD' + $Branching + $Matches.Step
    }
    else {
      $ResetArguments.Insert(0, $WorkingDirectory)
    }

    $WorkingDirectory = ''
  }

  if ($Tree) {
    $ResetArguments.Insert(0, $Tree)
  }
  $ResetArguments.Insert(0, '--hard')

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  Add-GitRepository @Repository

  $Repository.Verb = 'reset'
  Invoke-GitRepository @Repository @ResetArguments
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
    # Path to local repository. If not specified, defaults to the current location. The function will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory

  )

  $Private:ResetArguments = [List[string]]::new()

  if (
    $WorkingDirectory -and (
      $PWD | Resolve-GitRepository
    ) -and -not (
      $WorkingDirectory | Resolve-GitRepository
    )
  ) {
    $ResetArguments.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  $ResetArguments.AddRange([List[string]]$args)

  [hashtable]$Private:Repository = @{
    WorkingDirectory = $WorkingDirectory
  }
  Reset-GitRepository @Repository @ResetArguments

  Get-GitRepository @Repository
}
