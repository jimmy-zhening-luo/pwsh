@{
  RootModule           = 'Shell.Invoke.Workspace.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '58837453-25d1-453b-a77f-e37939368d68'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
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
    'i'
    'i.'
    'i..'
    'i~'
    'ic'
    'i/'
  )
}
