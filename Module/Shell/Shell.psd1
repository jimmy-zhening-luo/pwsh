@{
  RootModule           = 'Shell.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Start-Explorer'
    'Start-ExplorerSibling'
    'Start-ExplorerRelative'
    'Start-ExplorerHome'
    'Start-ExplorerCode'
    'Start-ExplorerDrive'
    'Start-Workspace'
    'Start-WorkspaceSibling'
    'Start-WorkspaceRelative'
    'Start-WorkspaceHome'
    'Start-WorkspaceCode'
    'Start-WorkspaceDrive'
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
    'i'
    'i.'
    'i..'
    'ih'
    'ic'
    'i/'
  )
}
