@{
  RootModule        = 'Shell.Clear.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '7b39ad83-781d-49f2-a378-f913d983b1a6'
  NestedModules     = @(
    '.\Shell.Clear.Directory'
  )
  FunctionsToExport = @(
    'Clear-Line'
    'Remove-Directory'
  )
  AliasesToExport   = @(
    'cl'
  )
}
