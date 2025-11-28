@{
  RootModule        = 'Invoke-Workspace.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '58837453-25d1-453b-a77f-e37939368d68'
  FunctionsToExport = @(
    'Invoke-Workspace'
    'Invoke-WorkspaceSibling'
    'Invoke-WorkspaceRelative'
    'Invoke-WorkspaceHome'
    'Invoke-WorkspaceCode'
    'Invoke-WorkspaceDrive'
  )
  AliasesToExport   = @(
    'i'
    'i.'
    'i..'
    'i~'
    'ic'
    'i/'
  )
}
