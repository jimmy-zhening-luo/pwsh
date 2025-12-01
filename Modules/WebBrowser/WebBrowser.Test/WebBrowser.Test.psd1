@{
  ModuleVersion     = '1.0.0.0'
  GUID              = 'f39e4440-9bb2-4182-af89-317087f740a4'
  NestedModules     = @(
    '.\WebBrowser.Test.Host'
    '.\WebBrowser.Test.Url'
  )
  FunctionsToExport = @(
    'Test-Host'
    'Test-Url'
  )
  AliasesToExport   = @(
    'tn'
    'tu'
  )
}
