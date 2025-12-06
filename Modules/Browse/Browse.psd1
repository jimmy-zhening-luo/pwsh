@{
  RootModule        = 'Browse.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'ff158ab2-d35c-450c-8086-03480d1b580e'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
    'GenericArgumentCompleter'
  )
  FunctionsToExport = @(
    'Test-Host'
    'Test-Url'
    'Open-Url'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'tn'
    'tu'
    'go'
    'open'
  )
}
