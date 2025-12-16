@{
  RootModule           = 'Shell.Invoke.Directory.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'ae5b665b-de9c-4872-824c-2e8bebc3abe0'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  RequiredModules     = @()
  FunctionsToExport    = @(
    'Invoke-Directory'
    'Invoke-DirectorySibling'
    'Invoke-DirectoryRelative'
    'Invoke-DirectoryHome'
    'Invoke-DirectoryCode'
    'Invoke-DirectoryDrive'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'e'
    'e.'
    'e..'
    'eh'
    'ec'
    'e/'
  )
}
