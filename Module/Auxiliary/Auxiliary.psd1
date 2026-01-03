@{
  RootModule           = 'Auxiliary.dll'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '0016d49d-7469-462c-b062-c382440f91f3'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @()
  CmdletsToExport      = @(
    'Copy-Guid'
    'ConvertTo-Hex'
    'Invoke-YouTubeDirectory'
    'Test-Cmdlet'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'gu'
    'guid'
    'hex'
    'yte'
    'test'
  )
}
