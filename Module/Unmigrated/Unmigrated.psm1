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
