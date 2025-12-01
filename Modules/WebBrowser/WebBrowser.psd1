@{
  RootModule        = 'WebBrowser.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = 'ff158ab2-d35c-450c-8086-03480d1b580e'
  NestedModules     = @(
    '.\WebBrowser.Test'
    '.\WebBrowser.Search'
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
    'go'
    'open'
    'tn'
    'tu'
    'search'
    'g'
    'maps'
    'map'
  )
}
