@{
  RootModule           = 'Shell.Clear.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '7b39ad83-781d-49f2-a378-f913d983b1a6'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Clear-Line'
    'Remove-Directory'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'cl'
  )
}
