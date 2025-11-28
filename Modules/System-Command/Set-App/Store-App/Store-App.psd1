@{
  RootModule           = 'Store-App.psm1'
  ModuleVersion        = '1.0.0.0'
  GUID                 = '16cbe1a0-f6d1-4b33-b9ef-4b5a4569888e'
  FunctionsToExport    = @(
    'Update-App'
  )
  AliasesToExport      = @(
    'su'
    'gapx'
    'remapx'
  )
  DefaultCommandPrefix = 'Store'
}
