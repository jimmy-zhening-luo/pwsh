@{
  RootModule           = 'Browse.dll'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '5a80488d-6a5a-4f24-b3f1-ff9d275ab8b5'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @()
  CmdletsToExport      = @(
    'Open-Url'
    'Test-Url'
    'Test-Host'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'o'
    'open'
    'tu'
    'tn'
  )
}
