@{
  RootModule        = '.\Git-Client.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'd8a43de0-682d-42d5-8333-bfbc80d84ac4'
  RequiredModules   = @(
    'Shell'
  )
  NestedModules     = @(
    '.\Add-Repository'
    '.\Clone-Repository'
    '.\Commit-Repository'
    '.\Pull-Repository'
    '.\Push-Repository'
    '.\Reset-Repository'
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
    'gitc'
    'gg'
    'gita'
    'gga'
    'gitcl'
    'gits'
    'ggs'
    'gitr'
    'gr'
    'gitrp'
    'grp'
    'gitm'
    'ggm'
    'gitp'
    'ggp'
    'gpa'
  )
}
