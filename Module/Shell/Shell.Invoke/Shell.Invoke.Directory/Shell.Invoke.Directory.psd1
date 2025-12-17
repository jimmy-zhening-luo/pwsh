@{
  RootModule           = 'Shell.Invoke.Directory.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'b86d4793-2876-48eb-8fc4-269f00a0e3d1'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
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
