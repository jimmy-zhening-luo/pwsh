@{
  RootModule        = 'Browse.Search.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'ca50e4fb-25d9-4503-b8ab-6139b5e5a79f'
  FunctionsToExport = @(
    'Expand-Query'
    'Search-Query'
    'Search-Map'
  )
  AliasesToExport   = @(
    'search'
    'g'
    'maps'
    'map'
  )
}
