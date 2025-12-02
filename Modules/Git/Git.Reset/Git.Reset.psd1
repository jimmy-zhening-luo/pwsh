@{
  RootModule        = 'Git.Reset.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'ba9a5388-7511-4093-828f-c889f540a2c1'
  FunctionsToExport = @(
    'Reset-Repository'
    'Restore-Repository'
  )
  AliasesToExport   = @(
    'gr'
    'grp'
  )
}
