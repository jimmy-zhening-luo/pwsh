@{
  RootModule            = 'Good.dll'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '3e553a20-19d7-42d7-aad6-3c819cab06ea'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredAssemblies    = @(
    'Good.dll'
  )
  FunctionsToExport     = @()
  CmdletsToExport       = @(
    'Test-Hello'
  )
  VariablesToExport     = @()
  AliasesToExport       = @()
}
