@{
  RootModule           = 'WindowsSystem.App.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '73ded6ab-d0a5-4d7c-a014-fa8b32a7714b'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  RequiredModules      = @()
  NestedModules        = @()
  FunctionsToExport    = @(
    'Update-StoreApp'
    'Update-WinGetApp'
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'su'
    'gapx'
    'remapx'
    'wget'
    'wgu'
    'wga'
    'wgf'
    'wgr'
  )
}
