@{
  RootModule            = 'Shell.Browse.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '1e45c553-ea48-41c2-a7fc-89b5c36f30b1'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
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
    'open'
  )
}
