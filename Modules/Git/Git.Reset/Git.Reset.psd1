@{
  RootModule        = 'Git.Reset.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'ba9a5388-7511-4093-828f-c889f540a2c1'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Reset-Repository'
    'Restore-Repository'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'gr'
    'grp'
  )
}
