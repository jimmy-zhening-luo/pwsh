@{
  RootModule           = 'PSAssembly.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '6c21e4fb-7a36-4659-9ac4-1f2f95553648'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Measure-PSProfile'
    'Invoke-PSHistory'
    'Invoke-PSProfile'
    'Update-PSProfile'
    'Restore-PSProfile'
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
