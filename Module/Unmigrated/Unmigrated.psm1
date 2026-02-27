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
          ![System.IO.Path]::IsPathRooted($wd) -and (
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
          ![System.IO.Path]::IsPathRooted($wd) -and (
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
    [Module.Commands.Code.WorkingDirectoryCompletions()]
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

  end {
    $GitArgument = [System.Collections.Generic.List[string]]::new()
    $Newable = $False

    if ($Verb) {
      if (
        [Module.Commands.Code.Git.GitVerb]::Verbs.Contains(
          $Verb.ToLower()
        )
      ) {
        if ($null -ne [Module.Commands.Code.Git.GitVerb+NewableVerb]::$Verb) {
          $Newable = $True

          if (
            [Module.Commands.Code.Git.GitArgument]::Regex().IsMatch(
              $WorkingDirectory
            )
          ) {
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
      $GitArgument.AddRange([string[]]$Argument)
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
}

<#
.LINK
https://git-scm.com/docs/git-status
#>
function Measure-GitRepository {

  [Alias('gg')]
  param(
    [Module.Commands.Code.WorkingDirectoryCompletions()]
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

    [Module.Commands.Code.WorkingDirectoryCompletions()]
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
  ) + ($RepositoryPathSegments -join '/')

  $CloneArgument = [System.Collections.Generic.List[string]]::new()
  $CloneArgument.Add($Origin)
  if ($args) {
    $CloneArgument.AddRange([string[]]$args)
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
    [Module.Commands.Code.WorkingDirectoryCompletions()]
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
  end {
    [string[]]$Repositories = Get-ChildItem -LiteralPath $HOME\code -Directory |
      Select-Object -ExpandProperty FullName |
      ForEach-Object -Process { Resolve-GitRepository -WorkingDirectory $PSItem }
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
}

<#
.LINK
https://git-scm.com/docs/git-diff
#>
function Compare-GitRepository {

  [Alias('gd')]
  param(
    [Module.Commands.Code.PathSpecCompletions()]
    # File pattern of files to diff, defaults to '.' (all)
    [string]$Name,

    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Repository path
    [string]$WorkingDirectory
  )

  $DiffArgument = [System.Collections.Generic.List[string]]::new()

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
    $DiffArgument.AddRange([string[]]$args)
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
    [Module.Commands.Code.PathSpecCompletions()]
    # File pattern of files to add, defaults to '.' (all)
    [string]$Name,

    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Repository path
    [string]$WorkingDirectory,

    # Equivalent to git add --renormalize flag
    [switch]$Renormalize
  )

  if (!$Name) {
    $Name = '.'
  }

  $AddArgument = [System.Collections.Generic.List[string]]::new()
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
    $AddArgument.AddRange([string[]]$args)
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
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Repository path
    [string]$WorkingDirectory,

    # Commit message. It must be non-empty except on an empty commit, where it defaults to 'No message.'
    [string]$Message,

    # Do not add unstaged nor untracked files; only commit files that are already staged
    [switch]$Staged,

    # Allow an empty commit, equivalent to git commit --allow-empty
    [switch]$AllowEmpty
  )

  $CommitArgument = [System.Collections.Generic.List[string]]::new()
  $Messages = [System.Collections.Generic.List[string]]::new()

  [string[]]$Argument, [string[]]$MessageWord = (
    $Message ? (, $Message + $args) : $args
  ).Where(
    {
      $PSItem
    }
  ).Where(
    {
      [Module.Commands.Code.Git.GitArgument]::Regex().IsMatch($PSItem)
    },
    'Split'
  )

  if ($Argument) {
    $CommitArgument.AddRange([string[]]$Argument)
  }
  if ($MessageWord) {
    $Messages.AddRange([string[]]$MessageWord)
  }

  if (
    $WorkingDirectory -and (
      Resolve-GitRepository -WorkingDirectory $PWD.Path
    ) -and !(
      Resolve-GitRepository -WorkingDirectory $WorkingDirectory
    )
  ) {
    if (
      [Module.Commands.Code.Git.GitArgument]::Regex().IsMatch(
        $WorkingDirectory
      ) -and -not $Messages.Count
    ) {
      $CommitArgument.Insert(
        0,
        $WorkingDirectory
      )
    }
    else {
      $Messages.Insert(
        0,
        $WorkingDirectory
      )
    }

    $WorkingDirectory = ''
  }

  if ($AllowEmpty -and '--allow-empty' -notin $CommitArgument) {
    $CommitArgument.Add(
      '--allow-empty'
    )
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
    [string[]]@(
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
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Repository path
    [string]$WorkingDirectory
  )

  $PushArgument = [System.Collections.Generic.List[string]]::new()

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
    $PushArgument.AddRange([string[]]$args)
  }

  Get-GitRepository -WorkingDirectory $WorkingDirectory

  Invoke-Git -Verb push -WorkingDirectory $WorkingDirectory -Argument $PushArgument
}

<#
.LINK
https://git-scm.com/docs/git-reset
#>
function Reset-GitRepository {

  [Alias('gr')]
  param(
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Repository path
    [string]$WorkingDirectory,

    # The tree spec to which to revert given as '[HEAD]([~]|^)[n]'. Defaults to HEAD. If only the number index is given, defaults to '~' branching. If only branching is given, defaults to index 0 (HEAD).
    [string]$Tree,

    # Non-destructive reset, equivalent to running git reset without --hard
    [switch]$Soft
  )

  $ResetArgument = [System.Collections.Generic.List[string]]::new()
  if ($args) {
    $ResetArgument.AddRange([string[]]$args)
  }

  if ($Tree) {
    $TreeMatch = [Module.Commands.Code.Git.GitArgument]::TreeRegex().Match($Tree)

    if (
      $TreeMatch.Success -and (
        $TreeMatch.Groups['step'].Value -eq '' -or $TreeMatch.Groups['step'].Value -as [int]
      )
    ) {
      [string]$Branching = $TreeMatch.Groups['branching'].Value -ne '' ? $TreeMatch.Groups['branching'].Value : '~'

      $Tree = 'HEAD' + $Branching + $TreeMatch.Groups['step'].Value
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
    if ($Tree) {
      $ResetArgument.Insert(0, $WorkingDirectory)
    }
    else {
      $TreeMatch = [Module.Commands.Code.Git.GitArgument]::TreeRegex().Match($WorkingDirectory)

      if (
        $TreeMatch.Success -and (
          $TreeMatch.Groups['step'].Value -eq '' -or $TreeMatch.Groups['step'].Value -as [int]
        )
      ) {
        [string]$Branching = $TreeMatch.Groups['branching'].Value -ne '' ? $TreeMatch.Groups['branching'].Value : '~'

        $Tree = 'HEAD' + $Branching + $TreeMatch.Groups['step'].Value
      }
      else {
        $ResetArgument.Insert(0, $WorkingDirectory)
      }
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
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Repository path
    [string]$WorkingDirectory
  )

  $ResetArgument = [System.Collections.Generic.List[string]]::new()

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
    $ResetArgument.AddRange([string[]]$args)
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

    [Parameter()]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Node package path
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments,
      DontShow
    )]
    # Additional npm arguments
    [string[]]$Argument,

    [Parameter()]
    # When npm command execution results in a non-zero exit code, write a warning and continue instead of the default behavior of throwing a terminating error.
    [switch]$NoThrow,

    [Parameter()]
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

  end {
    $NodeArgument = [System.Collections.Generic.List[string]]::new(
      [string[]]@(
        '--color=always'
      )
    )

    $NodeCommandArgument = [System.Collections.Generic.List[string]]::new()

    if ($Argument) {
      $NodeCommandArgument.AddRange([string[]]$Argument)
    }

    if ($WorkingDirectory) {
      if (
        [Module.Commands.Code.Node.NodeWorkingDirectory]::Test(
          $PWD.Path,
          $WorkingDirectory
        )
      ) {
        $PackagePrefix = $WorkingDirectory ? "--prefix=$((Resolve-Path $WorkingDirectory).Path)" : ''

        if ($PackagePrefix) {
          $NodeArgument.Add($PackagePrefix)
        }
      }
      else {
        $NodeCommandArgument.Insert(
          0,
          $WorkingDirectory
        )

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
      [string]$DeferredVerb = $NodeCommandArgument.Count ? $NodeCommandArgument.Find(
        {
          [Module.Commands.Code.Node.NodeVerb]::Verbs.Contains(
            $args[0].ToLower()
          )
        }
      ) : ''

      if ($DeferredVerb) {
        [void]$NodeCommandArgument.Remove($DeferredVerb)
      }

      $NodeCommandArgument.Insert(
        0,
        $Command
      )

      $Command = $DeferredVerb
    }

    if ($Command) {
      $NodeArgument.Add(
        $Command.ToLower()
      )
      if ($D) {
        $NodeCommandArgument.Add('-D')
      }
      if ($E) {
        $NodeCommandArgument.Add('-E')
      }
      if ($I) {
        $NodeCommandArgument.Add('-i')
      }
      if ($O) {
        $NodeCommandArgument.Add('-o')
      }
      if ($P) {
        $NodeCommandArgument.Add('-P')
      }
      if ($Version) {
        $NodeCommandArgument.Add('-v')
      }
    }
    else {
      if ($Version) {
        $NodeArgument.Add('-v')
      }
    }

    if ($NodeCommandArgument.Count) {
      $NodeArgument.AddRange($NodeCommandArgument)
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
  end {
    Invoke-Npm -Command cache -Argument @(
      'clean'
      '--force'
    )
  }
}

<#
.LINK
https://docs.npmjs.com/cli/commands/npm-outdated
#>
function Compare-NodeModule {
  [CmdletBinding()]
  [Alias('npo')]
  param(
    [Parameter(
      Position = 0
    )]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Node package path
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments,
      DontShow
    )]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Additional npm arguments
    [string[]]$Argument,

    [Parameter()]
    [Alias('a')]
    # In addition to direct dependencies, check for outdated meta-dependencies (--all)
    [switch]$All
  )

  end {
    $NodeArgument = [System.Collections.Generic.List[string]]::new()

    if ($Argument) {
      $NodeArgument.AddRange([string[]]$Argument)
    }

    if (
      $All -and -not $NodeArgument.Contains(
        '--all'
      )
    ) {
      $NodeArgument.Add('--all')
    }

    Invoke-Npm -Command outdated -WorkingDirectory $WorkingDirectory -NoThrow -Argument $NodeArgument
  }
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
    [Module.Commands.Code.Node.NodePackageVersionCompletions()]
    # New package version, default 'patch'
    [string]$Version,

    [Parameter(
      Position = 1
    )]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Node package path
    [string]$WorkingDirectory,

    [Parameter(
      Position = 2,
      ValueFromRemainingArguments,
      DontShow
    )]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Additional npm arguments
    [string[]]$Argument
  )

  end {
    $Version = switch ($Version) {
      '' {
        [Module.Commands.Code.Node.NodePackageVersion]::patch
        break
      }
      {
        $null -ne [Module.Commands.Code.Node.NodePackageVersion]::$Version
      } {
        [Module.Commands.Code.Node.NodePackageVersion]::$Version
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

    $NodeArgument = [System.Collections.Generic.List[string]]::new()
    $NodeArgument.Add(
      $Version.ToLower()
    )

    if (
      $WorkingDirectory -and ![Module.Commands.Code.Node.NodeWorkingDirectory]::Test(
        $PWD.Path,
        $WorkingDirectory
      )
    ) {
      $NodeArgument.Add($WorkingDirectory)
      $WorkingDirectory = ''
    }

    if ($Argument) {
      $NodeArgument.AddRange([string[]]$Argument)
    }

    Invoke-Npm -Command version -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
  }
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

    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Node package path
    [string]$WorkingDirectory
  )

  if (!$Script) {
    throw 'Script name is required.'
  }

  $NodeArgument = [System.Collections.Generic.List[string]]::new()
  $NodeArgument.Add($Script)

  if (
    $WorkingDirectory -and ![Module.Commands.Code.Node.NodeWorkingDirectory]::Test(
      $PWD.Path,
      $WorkingDirectory
    )
  ) {
    $NodeArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  if ($args) {
    $NodeArgument.AddRange([string[]]$args)
  }

  Invoke-Npm -Command run -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
}

<#
.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {
  [CmdletBinding()]
  [Alias('nt')]
  param(
    [Parameter(
      Position = 0
    )]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Node package path
    [string]$WorkingDirectory,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments,
      DontShow
    )]
    [Module.Commands.Code.WorkingDirectoryCompletions()]
    # Additional npm arguments
    [string[]]$Argument,

    [Parameter()]
    [Alias('i')]
    # Do not run scripts (--ignore-scripts). Commands explicitly intended to run a particular script, such as npm start, npm stop, npm restart, npm test, and npm run-script will still run their intended script if ignore-scripts is set, but they will not run any pre- or post-scripts.
    [switch]$IgnoreScript
  )

  end {
    $NodeArgument = [System.Collections.Generic.List[string]]::new()

    if ($Argument) {
      $NodeArgument.AddRange([string[]]$Argument)
    }

    if (
      $IgnoreScript -and -not $NodeArgument.Contains(
        '--ignore-scripts'
      )
    ) {
      $NodeArgument.Add('--ignore-scripts')
    }

    Invoke-Npm -Command test -WorkingDirectory $WorkingDirectory -Argument $NodeArgument
  }
}
