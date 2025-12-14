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
      ModuleName    = 'Completer'
      ModuleVersion = '3.0.0'
      GUID          = '95f00487-efc3-43e8-adcb-c539f28f0058'
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
