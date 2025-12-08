@{
  RootModule        = 'Git.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'd8a43de0-682d-42d5-8333-bfbc80d84ac4'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'GenericArgumentCompleter'
    'Shell'
  )
  FunctionsToExport = @(
    'Resolve-GitRepository'
    'Invoke-GitRepository'
    'Measure-GitRepository'
    'Import-GitRepository'
    'Get-GitRepository'
    'Get-ChildGitRepository'
    'Add-GitRepository'
    'Write-GitRepository'
    'Push-GitRepository'
    'Reset-GitRepository'
    'Restore-GitRepository'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'g'
    'gg'
    'gitcl'
    'gpp'
    'ga'
    'gs'
    'gr'
    'grp'
  )
}
