@{
  RootModule        = 'Browser-Client.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = 'ff158ab2-d35c-450c-8086-03480d1b580e'
  RequiredModules   = @(
    'Windows-Shell'
  )
  NestedModules     = @(
    '.\Test-Host'
    '.\Test-Url'
    '.\Search-Query'
  )
  FunctionsToExport = @(
    'Test-Host'
    'Test-Url'
    'Open-Url'
    'Expand-Query'
    'Search-Query'
    'Search-Map'
  )
  AliasesToExport   = @(
    'tn'
    'tu'
    'open'
    'go'
    'search'
    'g'
    'maps'
    'map'
  )
}
