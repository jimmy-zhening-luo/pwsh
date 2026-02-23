using namespace System.IO
using namespace System.Collections.Generic
using namespace Module.Completer
using namespace Module.Completer.Path

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

    [Parameter()]
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
              Join-Path $HOME code $wd
            ) -PathType Container
          )
        ) {
          Write-Output (
            Resolve-Path (
              Join-Path $HOME code $wd
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
              Join-Path $HOME code $wd .git
            ) -PathType Container
          )
        ) {
          Write-Output (
            Resolve-Path (
              Join-Path $HOME code $wd
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

$GIT_ARGUMENT = '^(?>(?=.*[*=])(?>.+)|-(?>\w|(?>-\w[-\w]*\w)))$'

<#
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
    [Module.Commands.Code.Git.GitVerbCompletionsAttribute()]
    # Git command
    [string]$Verb,

    [Parameter(
      Position = 1
    )]
    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Repository path. For all verbs except 'clone', 'config', and 'init', the command will throw an error if there is no Git repository at the path.
    [string]$WorkingDirectory,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    # Additional git arguments
    [string[]]$Argument,

    [Parameter()]
    # If git returns a non-zero exit code, warn and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    [Parameter(DontShow)]
    # Pass -d flag as git argument
    [switch]$d,

    [Parameter(DontShow)]
    # Pass -E flag as git argument
    [switch]$E,

    [Parameter(DontShow)]
    # Pass -i flag as git argument
    [switch]$I,

    [Parameter(DontShow)]
    # Pass -o flag as git argument
    [switch]$O,

    [Parameter(DontShow)]
    # Pass -P flag as git argument
    [switch]$P,

    [Parameter(DontShow)]
    # Pass -v flag as git argument
    [switch]$v
  )

  $GitArgument = [List[string]]::new()
  $Newable = $False

  if ($Verb) {
    if (
      [Module.Commands.Code.Git.GitVerb]::Verbs.Contains(
        $Verb.ToLower()
      )
    ) {
      if ($null -ne [Module.Commands.Code.Git.GitVerb+NewableVerb]::$Verb) {
        $Newable = $True

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

      $Verb = $Verb.ToLower()
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
    Newable          = $Newable
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
    # Repository path
    [string]$WorkingDirectory
  )

  Invoke-Git -Verb status -WorkingDirectory $WorkingDirectory -Argument $args
}

<#
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
    # Directory path into which to clone the repository. If the path points to a non-existant container, the container will be created. Throws a terminating error if container creation fails or git returns an error.
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
    # Repository path
    [string]$WorkingDirectory
  )

  Invoke-Git -Verb pull -WorkingDirectory $WorkingDirectory -Argument $args
}

<#
.LINK
https://git-scm.com/docs/git-pull
#>
function Get-ChildGitRepository {
  [CmdletBinding()]
  [Alias('gpp')]
  param()

  [string[]]$Repositories = Get-ChildItem -Path $HOME\code -Directory |
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
    # Repository path
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
    # Repository path
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
    # Repository path
    [string]$WorkingDirectory,

    # Commit message. It must be non-empty except on an empty commit, where it defaults to 'No message.'
    [string]$Message,

    # Do not add unstaged nor untracked files; only commit files that are already staged
    [switch]$Staged,

    # Allow an empty commit, equivalent to git commit --allow-empty
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
    # Repository path
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
    # Repository path
    [string]$WorkingDirectory,

    # The tree spec to which to revert given as '[HEAD]([~]|^)[n]'. Defaults to HEAD. If only the number index is given, defaults to '~' branching. If only branching is given, defaults to index 0 (HEAD).
    [string]$Tree,

    # Non-destructive reset, equivalent to running git reset without --hard
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

  [void]$ResetArgument.RemoveAll(
    {
      $args[0] -eq '--hard'
    }
  )
  if (-not $Soft) {
    $ResetArgument.Insert(0, '--hard')

    Add-GitRepository -WorkingDirectory $WorkingDirectory
  }

  Invoke-Git -Verb reset -WorkingDirectory $WorkingDirectory -Argument $ResetArgument
}

<#
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
    # Repository path
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

<#
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
    [Module.Commands.Code.Node.NodeVerbCompletionsAttribute()]
    # npm command verb
    [string]$Command,

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $True
    )]
    # Node package path
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments,
      DontShow
    )]
    # Additional npm arguments
    [string[]]$Argument,

    # When npm command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    # Show npm version if no command is specified. Otherwise, pass the -v flag as npm argument.
    [switch]$Version,

    [Parameter(DontShow)]
    # Pass -D flag as npm argument
    [switch]$D,

    [Parameter(DontShow)]
    # Pass -E flag as npm argument
    [switch]$E,

    [Parameter(DontShow)]
    # Pass -i flag as npm argument
    [switch]$I,

    [Parameter(DontShow)]
    # Pass -o flag as npm argument
    [switch]$O,

    [Parameter(DontShow)]
    # Pass -P flag as npm argument
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

  if ($WorkingDirectory) {
    if (
      Test-NodePackageDirectory -WorkingDirectory $WorkingDirectory
    ) {
      $PackagePrefix = Resolve-NodePackageDirectory -WorkingDirectory $WorkingDirectory

      if ($PackagePrefix) {
        $NodeArgument.Add($PackagePrefix)
      }
    }
    else {
      $NodeCommand.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }
  }

  if (
    $Command -and $Command.StartsWith(
      [char]'-'
    ) -or ![Module.Commands.Code.Node.NodeVerb]::Verbs.Contains(
      $Command.ToLower()
    ) -and ![Module.Commands.Code.Node.NodeVerb]::Aliases.ContainsKey(
      $Command.ToLower()
    )
  ) {
    [string]$DeferredVerb = $NodeCommand.Count ? $NodeCommand.Find(
      {
        [Module.Commands.Code.Node.NodeVerb]::Verbs.Contains(
          $args[0].ToLower()
        )
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
    # Node package path
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

<#
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
      [Module.Commands.Code.Node.NodePackageNamedVersion],
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
    # Node package path
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
      [Module.Commands.Code.Node.NodePackageNamedVersion]::patch
      break
    }
    {
      $null -ne [Module.Commands.Code.Node.NodePackageNamedVersion]::$Version
    } {
      [Module.Commands.Code.Node.NodePackageNamedVersion]::$Version
      break
    }
    default {
      [string]$SpecificVersion = $Version.StartsWith(
        [char]'v',
        [stringcomparison]::OrdinalIgnoreCase
      ) ? $Version.Substring(1) : $Version

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
    # Node package path
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
    # Node package path
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
