@{
  ModuleVersion     = '3.0.0.0'
  GUID              = 'e740b507-c756-4684-8565-8c579344e454'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'Shell.Set.Directory'
  )
  FunctionsToExport = @(
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
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
