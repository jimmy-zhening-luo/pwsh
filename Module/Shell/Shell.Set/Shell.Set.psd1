@{
  RootModule           = 'Shell.Set.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'afed7375-abb6-4d62-9b11-fa07320220aa'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'c'
    'c.'
    'c..'
    'ch'
    'cc'
    'c/'
    'd/'
  )
}
