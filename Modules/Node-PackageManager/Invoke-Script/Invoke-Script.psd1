@{
  RootModule        = 'Invoke-Script.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '013d50f2-8b5c-4e1c-b30e-8672041c1d32'
  RequiredModules   = @(
    'Argument-Completer'
  )
  FunctionsToExport = @(
    'Invoke-Script'
  )
  AliasesToExport   = @(
    'nr'
  )
}
