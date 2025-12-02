@{
  RootModule        = 'Browse.Search.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'ca50e4fb-25d9-4503-b8ab-6139b5e5a79f'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Expand-Query'
    'Search-Query'
    'Search-Map'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'search'
    'g'
    'maps'
    'map'
  )
}
