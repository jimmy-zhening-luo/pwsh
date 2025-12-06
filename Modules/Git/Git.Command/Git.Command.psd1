@{
  RootModule        = 'Git.Command.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'ba9a5388-7511-4093-828f-c889f540a2c1'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
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
    'gitcl'
    'ggp'
    'gpp'
    'gga'
    'ggm'
    'ggs'
    'gr'
    'grp'
  )
}
