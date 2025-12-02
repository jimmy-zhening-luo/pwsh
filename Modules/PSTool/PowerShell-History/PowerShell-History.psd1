@{
  RootModule        = 'PowerShell-History.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '6fd4487b-611b-4c0a-b124-f9c975b542d9'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
  )
  FunctionsToExport = @(
    'Invoke-PSHistory'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'oc'
  )
}
