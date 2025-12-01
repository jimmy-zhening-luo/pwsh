@{
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'e740b507-c756-4684-8565-8c579344e454'
  NestedModules     = @(
    '.\Shell.Set.Directory'
    '.\Shell.Set.File'
  )
  FunctionsToExport = @(
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
    'Set-File'
    'Set-FileSibling'
    'Set-FileRelative'
    'Set-FileHome'
    'Set-FileCode'
    'Set-FileDrive'
  )
  AliasesToExport   = @(
    'c'
    'c.'
    'c..'
    '..'
    '...'
    'c~'
    'cc'
    'c/'
    'd/'
    'w'
    'w.'
    'w..'
    'w~'
    'wc'
    'w/'
  )
}
