@{
  RootModule        = 'WindowsSystem.Setting.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'a733f473-0fc0-4a6c-a929-d55d3af424fd'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'Update-System'
    'Edit-System'
  )
  FunctionsToExport = @(
    'Update-Windows'
    'Edit-Path'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'wu'
    'path'
  )
}
