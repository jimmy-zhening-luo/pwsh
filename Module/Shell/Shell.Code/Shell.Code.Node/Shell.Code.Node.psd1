@{
  RootModule           = 'Shell.Code.Node.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '47836d9e-49bd-4405-bdc6-1900f3108d10'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
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
