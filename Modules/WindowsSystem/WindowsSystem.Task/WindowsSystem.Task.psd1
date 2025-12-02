@{
  RootModule        = 'WindowsSystem.Task.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '60e5aa07-6ab2-4f40-9f92-9d8fca12da66'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Stop-Task'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'sesv'
    'remsv'
    'tkill'
  )
}
