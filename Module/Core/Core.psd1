@{
  RootModule           = 'Core.dll'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'a2075667-de18-47e9-9804-bbf47f23131f'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @()
  CmdletsToExport      = @(
    'Edit-SystemPath'
    'Update-Windows'
    'Update-StoreApp'
    'Stop-Task'
    'Clear-Line'
    'Open-Url'
    'Test-Url'
    'Test-Host'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'path'
    'wu'
    'su'
    'tkill'
    'cl'
    'clear'
    'o'
    'open'
    'tu'
    'tn'
  )
}
