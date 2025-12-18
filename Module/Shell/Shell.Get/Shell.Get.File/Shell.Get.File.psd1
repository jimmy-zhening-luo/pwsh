@{
  RootModule           = 'Shell.Get.File.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '6212672b-6168-4bcb-a3bf-38291446571a'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @(
    'Get-File'
    'Get-FileSibling'
    'Get-FileRelative'
    'Get-FileHome'
    'Get-FileCode'
    'Get-FileDrive'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'p'
    'p.'
    'p..'
    'ph'
    'pc'
    'p/'
  )
}
