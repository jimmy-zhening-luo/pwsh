@{
  RootModule        = 'Shell.Set.File.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '2aa3293c-bd9b-48c5-9346-463b96967bb9'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Set-File'
    'Set-FileSibling'
    'Set-FileRelative'
    'Set-FileHome'
    'Set-FileCode'
    'Set-FileDrive'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'w'
    'w.'
    'w..'
    'w~'
    'wc'
    'w/'
  )
}
