@{
  RootModule           = 'Shell.Browse.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '1e45c553-ea48-41c2-a7fc-89b5c36f30b1'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      GUID          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
  )
  NestedModules        = @()
  FunctionsToExport    = @(
    'Test-Host'
    'Test-Url'
    'Open-Url'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'tn'
    'tu'
    'go'
    'open'
  )
}
