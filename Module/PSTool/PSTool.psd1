@{
  RootModule           = 'PSTool.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'c84491db-0b43-4dfc-80ea-890b16269a28'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Invoke-PSHistory'
    'Invoke-PSProfile'
    'Measure-PSProfile'
    'Update-PSProfile'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'oc'
    'op'
    'mc'
    'up'
  )
}
