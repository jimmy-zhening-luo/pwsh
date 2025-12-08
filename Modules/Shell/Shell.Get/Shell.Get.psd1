@{
  RootModule           = 'Shell.Get.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '83fda875-8f8c-4bce-a5da-e840d832378d'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      Guid          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
  )
  NestedModules        = @(
    @{
      ModuleName    = 'Shell.Get.Directory'
      ModuleVersion = '3.0.0.0'
      Guid          = '1c90175d-d43c-4e5e-9bd2-160c173da3e7'
    }
    @{
      ModuleName    = 'Shell.Get.File'
      ModuleVersion = '3.0.0.0'
      Guid          = '6212672b-6168-4bcb-a3bf-38291446571a'
    }
  )
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
    'l~'
    'lc'
    'l/'
    'p'
    'p.'
    'p..'
    'p~'
    'pc'
    'p/'
  )
}
