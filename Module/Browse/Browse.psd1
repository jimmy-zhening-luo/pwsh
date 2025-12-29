@{
  RootModule           = 'Browse.dll'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '5a80488d-6a5a-4f24-b3f1-ff9d275ab8b5'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  NestedModules        = @(
    @{
      ModuleName    = 'Browse.Test'
      ModuleVersion = '3.0.0'
      GUID          = 'f9754286-2e29-42e9-997c-221f9ce8e312'
    }
  )
  FunctionsToExport    = @(
    'Test-Host'
    'Test-Url'
  )
  CmdletsToExport      = @(
    'Open-Url'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'o'
    'open'
    'tn'
    'tu'
  )
}
