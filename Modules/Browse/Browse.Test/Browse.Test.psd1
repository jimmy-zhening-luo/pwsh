@{
  ModuleVersion     = '3.0.0.0'
  GUID              = 'f39e4440-9bb2-4182-af89-317087f740a4'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'Browse.Test.Host'
    'Browse.Test.Url'
  )
  FunctionsToExport = @(
    'Test-Host'
    'Test-Url'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'tn'
    'tu'
  )
}
