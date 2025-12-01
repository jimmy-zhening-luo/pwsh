@{
  RootModule        = 'Git.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'd8a43de0-682d-42d5-8333-bfbc80d84ac4'
  RequiredModules   = @(
    'Shell'
  )
  NestedModules     = @(
    'Add-Repository'
    'Clone-Repository'
    'Commit-Repository'
    'Pull-Repository'
    'Push-Repository'
    'Reset-Repository'
  )
  FunctionsToExport = @(
    'Invoke-Repository'
    'Resolve-Repository'
    'Add-Repository'
    'Import-Repository'
    'Write-Repository'
    'Get-Repository'
    'Get-ChildRepository'
    'Push-Repository'
    'Reset-Repository'
    'Restore-Repository'
  )
  AliasesToExport   = @(
    'gg'
    'gga'
    'gitcl'
    'ggs'
    'gr'
    'grp'
    'ggm'
    'ggp'
    'gpa'
  )
}
