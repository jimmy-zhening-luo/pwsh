@{
  RootModule        = 'Shell.Browse.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '1e45c553-ea48-41c2-a7fc-89b5c36f30b1'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
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
