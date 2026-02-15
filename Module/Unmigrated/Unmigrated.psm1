using namespace System.IO
using namespace System.Collections.Generic
using namespace Module.Completer
using namespace Module.Completer.Path

$ABOUT_BASE_URL = 'https://learn.microsoft.com/powershell/module/microsoft.powershell.core/about'

function Get-HelpOnline {
  [CmdletBinding()]
  [OutputType([System.Object])]
  [Alias('m', 'man')]
  param(

    [Parameter(
      Position = 0,
      ValueFromRemainingArguments
    )]
    [DynamicCompletions(
      {
        return (
          Import-PowerShellDataFile -Path $PSScriptRoot\PSHelpTopic.Local.psd1
        ).About + (
          (
            Get-ChildItem -Path Function:
          ).Name.ToLower() -notmatch '[^\w-]'
        ).ToLower()
      }
    )]
    [string[]]$Name,

    [string[]]$Parameter
  )

  if (!$Name) {
    return Get-Help -Name Get-Help
  }

  [string]$Topic = $Name -join '_'

  $HelpContent = ''
  $ArticleUrl = [List[uri]]::new()
  $Silent = @{
    ErrorAction    = 'SilentlyContinue'
    ProgressAction = 'SilentlyContinue'
  }

  $HelpArticleUrl = [List[uri]]::new()
  $HelpContent = Get-Help -Name $Topic @Silent

  if ($HelpContent -and $HelpContent.Count -ne 1) {
    $HelpContent = ''
  }

  if ($HelpContent) {
    $HelpContent.relatedLinks.navigationLink.Uri |
      Where-Object {
        ![string]::IsNullOrEmpty($PSItem)
      } |
      Where-Object {
        $PSItem -match '^(?=https?://\S|[-\w]+(?>\.[-\w]+)+/)'
      } |
      ForEach-Object {
        $PSItem -match '^(?=https?:)' ? $PSItem : "https://$PSItem"
      } |
      Where-Object {
        $PSItem -as [uri] -and (
          [uri]$PSItem
        ).IsAbsoluteUri
      } |
      ForEach-Object {
        $HelpArticleUrl.Add($PSItem)
      }
  }

  if ($HelpContent -and $Parameter) {
    $ParameterHelpContent = Get-Help -Name $Topic -Parameter $Parameter @Silent

    if ($ParameterHelpContent) {
      $HelpContent = $ParameterHelpContent

      if ($HelpArticleUrl.Count -eq 1 -and $Parameter.Count -eq 1) {
        $HelpArticleUrl[0] = [uri](
          [string]$HelpArticleUrl[0] + "#-$Parameter".ToLower()
        )
      }
    }
  }

  if ($HelpArticleUrl.Count) {
    $ArticleUrl.AddRange($HelpArticleUrl)
  }
  else {
    if ($HelpContent) {
      if (
        $HelpContent.Name -as [string] -and (
          [string]$HelpContent.Name
        ).StartsWith(
          'about_'
        )
      ) {
        $about_Article = [uri]"$ABOUT_BASE_URL/$([string]$HelpContent.Name)"
      }
    }
    else {
      function Resolve-AboutArticle {
        [CmdletBinding()]
        [OutputType([uri])]
        param(
          [Parameter()]
          [string]$Topic
        )

        $Test_about_Article = [uri]"$ABOUT_BASE_URL/$Topic"

        if (Test-Url -Uri $Test_about_Article) {
          return $Test_about_Article
        }
      }

      $about_Article = ''
      [string]$about_TopicCandidate = $Topic -replace '(?>[_ :]+)', '_' -replace '^(?>about)?_?', 'about_'

      $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate

      if (![string]$about_Article) {
        if ($about_TopicCandidate -notmatch 's$') {
          $about_TopicCandidate += 's'
          $about_Article = Resolve-AboutArticle -Topic $about_TopicCandidate
        }
      }

      if ([string]$about_Article) {
        $HelpContent = Get-Help -Name $about_TopicCandidate @Silent
        $ArticleUrl.Add($about_Article)
      }
    }
  }
}

if ($HelpContent) {
  Write-Output -InputObject $HelpContent
}

$Article = [List[uri]]::new()

if ($ArticleUrl.Count) {
  $ArticleUrl.ToArray() |
    ForEach-Object {
      'https://' + $PSItem.GetComponents(
        [uricomponents]::Host -bor [uricomponents]::Path -bor (
          $PSItem.Host -eq 'go.microsoft.com' ? [uricomponents]::Query : 0
        ) -bor [uricomponents]::Fragment,
        [uriformat]::Unescaped
      )
    } |
    ForEach-Object {
      $PSItem.StartsWith(
        'https://learn.microsoft.com/en-us/',
        [stringcomparison]::OrdinalIgnoreCase
      ) ? (
        $PSItem -replace '(?<=^https://learn\.microsoft\.com/)en-us/', ''
      ) : $PSItem
    } |
    Select-Object -Unique -CaseInsensitive |
    ForEach-Object {
      $Article.Add($PSItem)
    }
}

if ($Article.Count) {
  Write-Information -MessageData "$($Article.ToArray() -join "`n")" -InformationAction Continue
}

if (!$env:SSH_CLIENT) {
  if ($Article.Count) {
    $Article.ToArray() | Open-Url
  }
  else {
    if ($HelpContent) {
      $null = Get-Help -Name $Topic -Online @Silent 2>&1
    }
  }
}

function Resolve-GitRepository {
  [CmdletBinding()]
  [OutputType([string])]
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
        if (!$wd) {
          Write-Output $PWD.Path
        }
        elseif (Test-Path $wd -PathType Container) {
          Write-Output (
            Resolve-Path $wd
          ).Path
        }
        elseif (
          ![Path]::IsPathRooted(
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
        if (!$wd) {
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
          ![Path]::IsPathRooted(
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

  if (!$WorkingDirectory) {
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
    return $WorkingDirectory ? "--prefix=$((Resolve-Path $WorkingDirectory).Path)" : ''
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

.LINK
https://git-scm.com/docs
#>
function Invoke-Git {
  [CmdletBinding()]
  [Alias('g')]
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

    [Parameter(DontShow)]
    # Pass the -d flag as an argument to git
    [switch]$d,

    [Parameter(DontShow)]
    # Pass the -E flag as an argument to git
    [switch]$E,

    [Parameter(DontShow)]
    # Pass the -i flag as an argument to git
    [switch]$I,

    [Parameter(DontShow)]
    # Pass the -o flag as an argument to git
    [switch]$O,

    [Parameter(DontShow)]
    # Pass the -P flag as an argument to git
    [switch]$P,

    [Parameter(DontShow)]
    # Pass the -v flag as an argument to git
    [switch]$v
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
          $WorkingDirectory -and !(
            Resolve-GitRepository -WorkingDirectory $PWD.Path
          ) -and !(
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

  if (!$Repository) {
    if ($WorkingDirectory) {
      $GitArgument.Insert(0, $WorkingDirectory)

      $Resolve.WorkingDirectory = $PWD.Path
      $Repository = Resolve-GitRepository @Resolve
    }

    if (!$Repository) {
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
    $GitArgument.Add('-d')
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

.LINK
https://git-scm.com/docs/git-status
#>
function Measure-GitRepository {

  [Alias('gg')]
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

.LINK
https://git-scm.com/docs/git-clone
#>
function Import-GitRepository {

  [Alias('gitcl')]
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

  if (!$RepositoryPathSegments) {
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

.LINK
https://git-scm.com/docs/git-pull
#>
function Get-GitRepository {

  [Alias('gp')]
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

.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildGitRepository {
  [CmdletBinding()]
  [Alias('gpp')]
  param()

  [string[]]$Repositories = Get-ChildItem -Path $REPO_ROOT -Directory |
    Select-Object -ExpandProperty FullName |
    Resolve-GitRepository
  $Count = $Repositories.Count

  Write-Progress -Activity Pull -Status "0/$Count" -PercentComplete 0

  $i = 0
  foreach ($Repository in $Repositories) {
    Get-GitRepository -WorkingDirectory $Repository

    ++$i
    Write-Progress -Activity Pull -Status "$i/$Count" -PercentComplete ($i * 100 / $Count)
  }

  return "`nPulled $Count repositor" + ($Count -eq 1 ? 'y' : 'ies')
}

<#
.SYNOPSIS
Use Git to diff the current local working tree against the current local index.

.DESCRIPTION
This function is an alias for 'git diff [path]'.

.LINK
https://git-scm.com/docs/git-diff
#>
function Compare-GitRepository {

  [Alias('gd')]
  param(

    [PathCompletions(
      '',
      [PathItemType]::File
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
    ) -and !(
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    $DiffArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
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

.LINK
https://git-scm.com/docs/git-add
#>
function Add-GitRepository {

  [Alias('ga')]
  param(

    [PathCompletions(
      '',
      [PathItemType]::File
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

  if (!$Name) {
    $Name = '.'
  }

  $AddArgument = [List[string]]::new()
  $AddArgument.Add($Name)

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and !(
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    $AddArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
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

.LINK
https://git-scm.com/docs/git-commit
#>
function Write-GitRepository {

  [Alias('gm')]
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
    ) -and !(
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    if ($WorkingDirectory -match $GIT_ARGUMENT -and !$Messages.Count) {
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

  if (!$Messages.Count) {
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

  if (!$Staged) {
    Add-GitRepository -WorkingDirectory $WorkingDirectory
  }

  Invoke-Git -Verb commit -WorkingDirectory $WorkingDirectory -Argument $CommitArgument
}

<#
.SYNOPSIS
Use Git to push changes to a repository.

.DESCRIPTION
This function is an alias for 'git push'.

.LINK
https://git-scm.com/docs/git-push
#>
function Push-GitRepository {

  [Alias('gs')]
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
    ) -and !(
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

  Invoke-Git -Verb push -WorkingDirectory $WorkingDirectory -Argument $PushArgument
}

$TREE_SPEC = '^(?=.)(?>HEAD)?(?<Branching>(?>~|\^)?)(?<Step>(?>\d{0,10}))$'

<#
.SYNOPSIS
Use Git to undo changes in a repository.

.DESCRIPTION
This function is an alias for 'git add . && git reset --hard [HEAD]([~]|^)[n]'.

.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-GitRepository {

  [Alias('gr')]
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
        !$Matches.Step -or $Matches.Step -as [int]
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
    ) -and !(
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    if (
      !$Tree -and $WorkingDirectory -match $TREE_SPEC -and (
        !$Matches.Step -or $Matches.Step -as [int]
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

  Invoke-Git -Verb reset -WorkingDirectory $WorkingDirectory -Argument $ResetArgument
}

<#
.SYNOPSIS
Use Git to restore a repository to its previous state.

.DESCRIPTION
This function is an alias for 'git add . && git reset --hard && git pull'.

.LINK
https://git-scm.com/docs/git-reset
#>
function Restore-GitRepository {

  [Alias('grp')]
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
    ) -and !(
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

.LINK
https://docs.npmjs.com/cli/commands

.LINK
https://docs.npmjs.com/cli/commands/npm
#>
function Invoke-Npm {
  [CmdletBinding()]
  [Alias('n')]
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
      ValueFromRemainingArguments,
      DontShow
    )]
    # Additional arguments to pass to npm
    [string[]]$Argument,

    # When npm command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    # Show npm version if no command is specified. Otherwise, pass the -v flag.
    [switch]$Version,

    [Parameter(DontShow)]
    # Pass the -D flag as an argument to npm
    [switch]$D,

    [Parameter(DontShow)]
    # Pass the -E flag as an argument to npm
    [switch]$E,

    [Parameter(DontShow)]
    # Pass the -i flag as an argument to npm
    [switch]$I,

    [Parameter(DontShow)]
    # Pass the -o flag as an argument to npm
    [switch]$O,

    [Parameter(DontShow)]
    # Pass the -P flag as an argument to npm
    [switch]$P
  )

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add('--color=always')

  $NodeCommand = [List[string]]::new()
  if ($Argument) {
    $NodeCommand.AddRange(
      [List[string]]$Argument
    )
  }

  if ($WorkingDirectory.Length) {
    if (
      $WorkingDirectory.StartsWith([char]'-') -or !(
        Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory
      )
    ) {
      $NodeCommand.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
    else {
      $PackagePrefix = Resolve-NodePackageDirectory -WorkingDirectory $WorkingDirectory

      if ($PackagePrefix) {
        $NodeArgument.Add($PackagePrefix)
      }
    }
  }

  if ($Command.Length -and $Command.StartsWith([char]'-') -or $Command -notin $NODE_VERB -and !$NODE_ALIAS.ContainsKey($Command)) {
    [string]$DeferredVerb = $NodeCommand.Count ? $NodeCommand.Find(
      {
        $args[0] -in $NODE_VERB
      }
    ) : ''

    if ($DeferredVerb) {
      [void]$NodeCommand.Remove($DeferredVerb)
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

  if ($NodeCommand.Count) {
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

.LINK
https://nodejs.org/api/cli.html
#>
function Invoke-Node {

  [Alias('no')]
  param()

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

.LINK
https://docs.npmjs.com/cli/commands/npx
#>
function Invoke-NodeExecutable {

  [Alias('nx')]
  param()

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

.LINK
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {
  [CmdletBinding()]
  [Alias('ncc')]
  param()

  $NodeArgument = [List[string]]::new(
    [List[string]]@(
      'clean'
      '--force'
    )
  )

  Invoke-Npm -Command cache -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to check for outdated packages in a Node package.

.DESCRIPTION
This function is an alias for 'npm outdated [--prefix $WorkingDirectory]'.

.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-NodeModule {

  [Alias('npo')]
  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory
  )

  $NodeArgument = [List[string]]::new()

  if ($WorkingDirectory) {
    if (
      !(Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)
    ) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
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

.LINK
https://docs.npmjs.com/cli/commands/npm-version
#>
function Step-NodePackageVersion {

  [CmdletBinding()]
  [Alias('nu')]
  param(

    [Parameter(
      Position = 0
    )]
    [EnumCompletions(
      [NodePackageNamedVersion],
      $False,
      [CompletionCase]::Preserve
    )]
    # New package version, default 'patch'
    [string]$Version,

    [Parameter(
      Position = 1
    )]
    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [string[]]$Argument
  )

  $Version = switch ($Version) {
    '' {
      [NodePackageNamedVersion]::patch
      break
    }
    {
      $null -ne [NodePackageNamedVersion]::$Version
    } {
      [NodePackageNamedVersion]::$Version
      break
    }
    default {
      [string]$SpecificVersion = $Version.StartsWith(
        [char]'v',
        [stringcomparison]::OrdinalIgnoreCase
      ) ?  $Version.Substring(1) : $Version

      [semver]$Semver = $null

      if ([semver]::TryParse($SpecificVersion, [ref]$Semver)) {
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
    if (
      !(Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)
    ) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($Argument) {
    $NodeArgument.AddRange(
      [List[string]]$Argument
    )
  }

  Invoke-Npm -Command version -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

<#
.SYNOPSIS
Use Node Package Manager (npm) to run a script defined in a Node package's 'package.json'.

.DESCRIPTION
This function is an alias for 'npm run [script] [--prefix $WorkingDirectory] [--args]'.

.LINK
https://docs.npmjs.com/cli/commands/npm-run
#>
function Invoke-NodePackageScript {

  [Alias('nr')]
  param(

    # Name of the npm script to run
    [string]$Script,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory
  )

  if (!$Script) {
    throw 'Script name is required.'
  }

  $NodeArgument = [List[string]]::new()
  $NodeArgument.Add($Script)

  if ($WorkingDirectory) {
    if (
      !(Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)
    ) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
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

.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {

  [Alias('nt')]
  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package root at which to run the command
    [string]$WorkingDirectory
  )

  $NodeArgument = [List[string]]::new()

  if ($WorkingDirectory) {
    if (
      !(Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory)
    ) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if ($args) {
    $NodeArgument.AddRange(
      [List[string]]$args
    )
  }

  Invoke-Npm -Command test -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}
