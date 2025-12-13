@{
  RootModule            = 'Shell.Get.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '83fda875-8f8c-4bce-a5da-e840d832378d'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  NestedModules         = @(
    @{
      ModuleName    = 'Shell.Get.Directory'
      ModuleVersion = '3.0.0.0'
      GUID          = '1c90175d-d43c-4e5e-9bd2-160c173da3e7'
    }
    @{
      ModuleName    = 'Shell.Get.File'
      ModuleVersion = '3.0.0.0'
      GUID          = '6212672b-6168-4bcb-a3bf-38291446571a'
    }
  )
  FunctionsToExport     = @(
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
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'split'
    'hash'
    'size'
    'sz'
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
