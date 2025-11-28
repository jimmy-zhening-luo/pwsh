@{
  ModuleVersion     = '1.0.0.0'
  GUID              = 'e740b507-c756-4684-8565-8c579344e454'
  NestedModules     = @(
    '.\Update-File'
  )
  FunctionsToExport = @(
    'Update-File'
    'Update-FileSibling'
    'Update-FileRelative'
    'Update-FileHome'
    'Update-FileCode'
    'Update-FileDrive'
  )
  AliasesToExport   = @(
    'w'
    'w.'
    'w..'
    'w~'
    'wc'
    'w/'
  )
}
