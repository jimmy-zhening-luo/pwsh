@{
  RootModule           = 'Builder.psm1'
  ModuleVersion        = '7.6.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'ba559236-b79d-4b21-8e06-d5c0d5d5ce26'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.6'
  FunctionsToExport    = @(
    'Update-PSProfile'
    'Build-PSProfile'
    'Restore-PSProfile'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'up'
    'upp'
    'upr'
  )
}
