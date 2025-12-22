@{
  RootModule           = 'Shell.Start.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '043fee19-632c-4e0a-ac64-31b9fe02be00'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
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
