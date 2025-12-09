@{
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = 'e740b507-c756-4684-8565-8c579344e454'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  NestedModules         = @(
    @{
      ModuleName    = 'Shell.Set.Directory'
      ModuleVersion = '3.0.0.0'
      GUID          = 'b6522962-e911-4dc4-bfbe-0181264ad7d2'
    }
  )
  FunctionsToExport     = @(
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'c'
    'c.'
    'c..'
    '..'
    '...'
    'c~'
    'cc'
    'c/'
    'd/'
  )
}
