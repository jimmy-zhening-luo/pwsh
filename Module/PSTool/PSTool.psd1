@{
  RootModule           = 'PSTool.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'c84491db-0b43-4dfc-80ea-890b16269a28'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @(
    'Measure-PSProfile'
    'Invoke-PSHistory'
    'Invoke-PSProfile'
    'Update-PSProfile'
    'Restore-PSProfile'
    'Write-PSModuleCache'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'mcp'
    'oc'
    'op'
    'up'
    'upp'
  )
}
