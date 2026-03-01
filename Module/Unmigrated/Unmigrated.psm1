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
