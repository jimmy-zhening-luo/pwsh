@{
  RootModule           = 'Code.Node.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '886a95dd-cf5d-4016-8015-5f9a9345d66c'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  FunctionsToExport    = @(
    'Invoke-Node'
    'Invoke-NodePackage'
    'Invoke-NodeExecutable'
    'Clear-NodeModuleCache'
    'Compare-NodeModule'
    'Step-NodePackageVersion'
    'Invoke-NodePackageScript'
    'Test-NodePackage'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'no'
    'n'
    'nx'
    'ncc'
    'npo'
    'nu'
    'nr'
    'nt'
  )
}
