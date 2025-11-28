@{
  ModuleVersion     = '1.0.0.0'
  GUID              = '83fda875-8f8c-4bce-a5da-e840d832378d'
  NestedModules     = @(
    '.\Get-Directory'
    '.\Get-File'
  )
  FunctionsToExport = @(
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
  AliasesToExport   = @(
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
