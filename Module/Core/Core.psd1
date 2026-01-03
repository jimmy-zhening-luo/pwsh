@{
  RootModule           = 'Core.dll'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'a2075667-de18-47e9-9804-bbf47f23131f'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @()
  CmdletsToExport      = @(
    'Clear-Line'
    'Update-Windows'
    'Update-StoreApp'
    'Open-Url'
    'Test-Url'
    'Test-Host'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'cl'
    'clear'
    'wu'
    'su'
    'o'
    'open'
    'tu'
    'tn'
  )
}
