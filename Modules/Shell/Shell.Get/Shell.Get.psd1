@{
  RootModule           = 'Shell.Get.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '83fda875-8f8c-4bce-a5da-e840d832378d'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    'GenericArgumentCompleter'
  )
  NestedModules        = @(
    'Shell.Get.Directory'
    'Shell.Get.File'
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
