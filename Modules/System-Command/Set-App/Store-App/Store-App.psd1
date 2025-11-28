@{
  RootModule        = 'Store-App.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = 'ef562773-6f1b-4a25-9e5f-d5ceba7b5752'
  NestedModules     = @(
    '.\Update-StoreApp'
  )
  FunctionsToExport = @(
    'Update-StoreApp'
  )
  AliasesToExport   = @(
    'gapx'
    'remapx'
    'su'
  )
}
