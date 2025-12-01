@{
  RootModule        = 'Sandbox.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '0cd1efe3-3183-4a36-9711-93ac269ca118'
  RequiredModules   = @(
    'Shell'
  )
  FunctionsToExport = @(
    'Test-Sandbox'
  )
  AliasesToExport   = @(
    'sand'
    'sandbox'
  )
}
