@{
  RootModule           = 'Shell.Invoke.Workspace.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'e1e5a8be-a253-41f6-a38e-a13435965385'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @(
    'Invoke-Workspace'
    'Invoke-WorkspaceSibling'
    'Invoke-WorkspaceRelative'
    'Invoke-WorkspaceHome'
    'Invoke-WorkspaceCode'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'i'
    'i.'
    'i..'
    'ih'
    'ic'
  )
}
