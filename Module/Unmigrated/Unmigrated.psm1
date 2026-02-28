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

  Invoke-Git -Verb clone -WorkingDirectory $WorkingDirectory -ArgumentList $CloneArgument
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
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $PWD.Path)
    ) -and !(
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $WorkingDirectory)
    )
  ) {
    $DiffArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  if ($args) {
    $DiffArgument.AddRange([string[]]$args)
  }

  Invoke-Git -Verb diff -WorkingDirectory $WorkingDirectory -ArgumentList $DiffArgument
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
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $PWD.Path)
    ) -and !(
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $WorkingDirectory)
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

  Invoke-Git -Verb add -WorkingDirectory $WorkingDirectory -ArgumentList $AddArgument
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

  [string[]]$ArgumentList, [string[]]$MessageWord = (
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

  if ($ArgumentList) {
    $CommitArgument.AddRange([string[]]$ArgumentList)
  }
  if ($MessageWord) {
    $Messages.AddRange([string[]]$MessageWord)
  }

  if (
    $WorkingDirectory -and (
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $PWD.Path)
    ) -and !(
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $WorkingDirectory)
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

  Invoke-Git -Verb commit -WorkingDirectory $WorkingDirectory -ArgumentList $CommitArgument
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
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $PWD.Path)
    ) -and !(
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $WorkingDirectory)
    )
  ) {
    $PushArgument.Add($WorkingDirectory)
    $WorkingDirectory = ''
  }

  if ($args) {
    $PushArgument.AddRange([string[]]$args)
  }

  Get-GitRepository -WorkingDirectory $WorkingDirectory

  Invoke-Git -Verb push -WorkingDirectory $WorkingDirectory -ArgumentList $PushArgument
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
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $PWD.Path)
    ) -and !(
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $WorkingDirectory)
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

  Invoke-Git -Verb reset -WorkingDirectory $WorkingDirectory -ArgumentList $ResetArgument
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
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $PWD.Path)
    ) -and !(
      [Module.Commands.Code.Git.GitWorkingDirectory]::Resolve($PWD.Path, $WorkingDirectory)
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
https://docs.npmjs.com/cli/commands/npm-cache
#>
function Clear-NodeModuleCache {
  [CmdletBinding()]
  [Alias('ncc')]
  param()
  end {
    Invoke-Npm -Verb cache -ArgumentList @(
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
    [Parameter(Position = 0)]
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
    [string[]]$ArgumentList,

    [Parameter()]
    [Alias('a')]
    # In addition to direct dependencies, check for outdated meta-dependencies (--all)
    [switch]$All
  )

  end {
    $NodeArgument = [System.Collections.Generic.List[string]]::new()

    if ($ArgumentList) {
      $NodeArgument.AddRange([string[]]$ArgumentList)
    }

    if ($All -and !$NodeArgument.Contains('--all')) {
      $NodeArgument.Add('--all')
    }

    Invoke-Npm -Verb outdated -WorkingDirectory $WorkingDirectory -NoThrow -ArgumentList $NodeArgument
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
    [Parameter(Position = 0)]
    [Module.Commands.Code.Node.NodePackageVersionCompletions()]
    # New package version, default 'patch'
    [string]$Version,

    [Parameter(Position = 1)]
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
    [string[]]$ArgumentList
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

    if ($ArgumentList) {
      $NodeArgument.AddRange([string[]]$ArgumentList)
    }

    Invoke-Npm -Verb version -WorkingDirectory $WorkingDirectory -ArgumentList $NodeArgument
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

  Invoke-Npm -Verb run -WorkingDirectory $WorkingDirectory -ArgumentList $NodeArgument
}

<#
.LINK
https://docs.npmjs.com/cli/commands/npm-test
#>
function Test-NodePackage {
  [CmdletBinding()]
  [Alias('nt')]
  param(
    [Parameter(Position = 0)]
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
    [string[]]$ArgumentList,

    [Parameter()]
    [Alias('i')]
    # Do not run scripts (--ignore-scripts). Commands explicitly intended to run a particular script, such as npm start, npm stop, npm restart, npm test, and npm run-script will still run their intended script if ignore-scripts is set, but they will not run any pre- or post-scripts.
    [switch]$IgnoreScript
  )

  end {
    $NodeArgument = [System.Collections.Generic.List[string]]::new()

    if ($ArgumentList) {
      $NodeArgument.AddRange([string[]]$ArgumentList)
    }

    if (
      $IgnoreScript -and !$NodeArgument.Contains(
        '--ignore-scripts'
      )
    ) {
      $NodeArgument.Add('--ignore-scripts')
    }

    Invoke-Npm -Verb test -WorkingDirectory $WorkingDirectory -ArgumentList $NodeArgument
  }
}
