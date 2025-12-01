@{
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'a733f473-0fc0-4a6c-a929-d55d3af424fd'
  NestedModules     = @(
    'Update-System'
    'Edit-System'
  )
  FunctionsToExport = @(
    'Update-Windows'
    'Edit-Path'
  )
  AliasesToExport   = @(
    'wu'
    'path'
  )
}
