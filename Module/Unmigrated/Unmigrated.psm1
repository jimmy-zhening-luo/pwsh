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
