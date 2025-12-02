@{
  RootModule        = 'Store-App.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '16cbe1a0-f6d1-4b33-b9ef-4b5a4569888e'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Update-StoreApp'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'su'
    'gapx'
    'remapx'
  )
}
