@{
  RootModule        = 'Stop-Task.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '0c74786c-ec51-4189-830f-4e03f92ca021'
  FunctionsToExport = @(
    'Stop-Task'
  )
  AliasesToExport   = @(
    'tkill'
    'tkillx'
  )
}
