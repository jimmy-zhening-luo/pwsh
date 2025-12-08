@{
  RootModule           = 'Shell.New.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '9a3324b2-207d-4635-a8d6-ac8261181fb1'
  PowerShellVersion    = '7.5'
  RequiredModules      = @()
  NestedModules        = @()
  FunctionsToExport    = @(
    'New-Directory'
    'New-Junction'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'touch'
    'mk'
    'mj'
  )
}
