@{
  RootModule            = 'Browse.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '5a80488d-6a5a-4f24-b3f1-ff9d275ab8b5'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredModules       = @(
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      GUID          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
  )
  FunctionsToExport     = @(
    'Test-Host'
    'Test-Url'
    'Open-Url'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'tn'
    'tu'
    'go'
  )
}
