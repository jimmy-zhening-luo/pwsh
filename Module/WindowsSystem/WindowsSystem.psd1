@{
  RootModule           = 'WindowsSystem.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'b047ad82-dcbf-48cc-876a-78c6334900af'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  NestedModules        = @(
    @{
      ModuleName    = 'WindowsSystem.App'
      ModuleVersion = '3.0.0'
      GUID          = '73ded6ab-d0a5-4d7c-a014-fa8b32a7714b'
    }
  )
  FunctionsToExport    = @(
    'Invoke-CommandPrompt'
    'Stop-Task'
    'Edit-SystemPath'
    'Update-WinGetApp'
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'restart'
    'sesv'
    'remsv'
    'tkill'
    'path'
    'gapx'
    'remapx'
    'wget'
    'wgu'
    'wga'
    'wgf'
    'wgr'
  )
}
