@{
  RootModule        = 'Git.Command.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'ba9a5388-7511-4093-828f-c889f540a2c1'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
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
    'gitcl'
    'gpp'
    'ga'
    'gs'
    'gr'
    'grp'
  )
}
