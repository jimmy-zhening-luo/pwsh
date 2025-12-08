@{
  RootModule           = 'Shell.Set.Directory.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'b6522962-e911-4dc4-bfbe-0181264ad7d2'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'c'
    'c.'
    'c..'
    'c~'
    'cc'
    'c/'
    'd/'
  )
}
