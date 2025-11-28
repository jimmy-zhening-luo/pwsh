@{
  RootModule        = 'Set-Directory.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = 'b6522962-e911-4dc4-bfbe-0181264ad7d2'
  FunctionsToExport = @(
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
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
  )
}
