@{
  RootModule        = 'Browse.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'ff158ab2-d35c-450c-8086-03480d1b580e'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'Browse.Test'
    'Browse.Search'
  )
  FunctionsToExport = @(
    'Test-Host'
    'Test-Url'
    'Open-Url'
    'Expand-Query'
    'Search-Query'
    'Search-Map'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
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
