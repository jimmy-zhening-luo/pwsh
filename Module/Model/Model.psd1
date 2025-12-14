@{
  RootModule            = 'Model.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '95295970-cd48-492b-83f9-f2327927e0ca'
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
  FunctionsToExport     = @()
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    '^'
    't'
    'to'
    'k'
    'key'
    'count'
    'z'
    'format'
  )
}
