@{
  RootModule           = 'Shell.Get.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '83fda875-8f8c-4bce-a5da-e840d832378d'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @(
    'Get-Size'
    'Get-Directory'
    'Get-DirectorySibling'
    'Get-DirectoryRelative'
    'Get-DirectoryHome'
    'Get-DirectoryCode'
    'Get-DirectoryDrive'
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
    'split'
    'hash'
    'sz'
    'size'
    'l'
    'l.'
    'l..'
    'lh'
    'lc'
    'l/'
    'p'
    'p.'
    'p..'
    'ph'
    'pc'
    'p/'
  )
}
