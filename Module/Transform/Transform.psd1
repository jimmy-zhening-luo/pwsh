@{
  RootModule           = 'Transform.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '33022719-1f6c-4844-b018-df7897bece12'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  NestedModules        = @(
    'Transform.dll'
  )
  FunctionsToExport    = @(
    'Test-Function'
  )
  CmdletsToExport      = @(
    'Test-Cmdlet'
    'ConvertTo-Hex'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'fest'
    'test'
    'hex'
  )
}
