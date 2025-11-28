@{
  RootModule        = 'Update-File.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '2aa3293c-bd9b-48c5-9346-463b96967bb9'
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
