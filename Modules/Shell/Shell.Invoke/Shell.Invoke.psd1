@{
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '130b56d3-229a-4ec1-be75-d31a615147c8'
  PowerShellVersion    = '7.5'
  NestedModules        = @(
    'Shell.Invoke.Directory'
    'Shell.Invoke.Workspace'
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
}
