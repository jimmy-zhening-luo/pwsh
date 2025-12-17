@{
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '6f610e9b-3218-49e5-93e7-f9242095f7c9'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  NestedModules        = @(
    @{
      ModuleName    = 'Shell.Invoke.Directory'
      ModuleVersion = '3.0.0.0'
      GUID          = 'b86d4793-2876-48eb-8fc4-269f00a0e3d1'
    }
    @{
      ModuleName    = 'Shell.Invoke.Workspace'
      ModuleVersion = '3.0.0.0'
      GUID          = 'e1e5a8be-a253-41f6-a38e-a13435965385'
    }
  )
  FunctionsToExport    = @(
    'Invoke-Directory'
    'Invoke-DirectorySibling'
    'Invoke-DirectoryRelative'
    'Invoke-DirectoryHome'
    'Invoke-DirectoryCode'
    'Invoke-DirectoryDrive'
    'Invoke-Workspace'
    'Invoke-WorkspaceSibling'
    'Invoke-WorkspaceRelative'
    'Invoke-WorkspaceHome'
    'Invoke-WorkspaceCode'
    'Invoke-WorkspaceDrive'
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
