@{
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '130b56d3-229a-4ec1-be75-d31a615147c8'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredModules       = @()
  NestedModules         = @(
    @{
      ModuleName    = 'Shell.Invoke.Directory'
      ModuleVersion = '3.0.0.0'
      GUID          = 'ae5b665b-de9c-4872-824c-2e8bebc3abe0'
    }
    @{
      ModuleName    = 'Shell.Invoke.Workspace'
      ModuleVersion = '3.0.0.0'
      GUID          = '58837453-25d1-453b-a77f-e37939368d68'
    }
  )
  FunctionsToExport     = @(
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
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'e'
    'e.'
    'e..'
    'e~'
    'ec'
    'e/'
    'i'
    'i.'
    'i..'
    'i~'
    'ic'
    'i/'
  )
  ModuleList            = @(
    @{
      ModuleName    = 'Shell.Invoke.Directory'
      ModuleVersion = '3.0.0.0'
      GUID          = 'ae5b665b-de9c-4872-824c-2e8bebc3abe0'
    }
    @{
      ModuleName    = 'Shell.Invoke.Workspace'
      ModuleVersion = '3.0.0.0'
      GUID          = '58837453-25d1-453b-a77f-e37939368d68'
    }
  )
}
