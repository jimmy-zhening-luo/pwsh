@{
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'f39e4440-9bb2-4182-af89-317087f740a4'
  NestedModules     = @(
    '.\Browse.Test.Host'
    '.\Browse.Test.Url'
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
