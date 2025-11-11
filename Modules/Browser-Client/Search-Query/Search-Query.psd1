@{
  RootModule        = 'Search-Query.psm1'
  ModuleVersion     = '1.0.0.0'
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
