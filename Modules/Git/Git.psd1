@{
  RootModule        = 'Git.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'd8a43de0-682d-42d5-8333-bfbc80d84ac4'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
    'GenericArgumentCompleter'
  )
  NestedModules     = @(
    'Git.Command'
  )
  FunctionsToExport = @(
    'Resolve-Repository'
    'Invoke-Repository'
    'Import-Repository'
    'Get-Repository'
    'Get-ChildRepository'
    'Add-Repository'
    'Write-Repository'
    'Push-Repository'
    'Reset-Repository'
    'Restore-Repository'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'gg'
    'gitcl'
    'ggp'
    'gpa'
    'gga'
    'ggm'
    'ggs'
    'gr'
    'grp'
  )
}
