@{
  RootModule            = 'Node.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '7e838ead-1871-47f2-a845-3ce3725b7781'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredModules       = @(
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      GUID          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
    @{
      ModuleName    = 'PathArgumentCompleter'
      ModuleVersion = '3.0.0'
      GUID          = '4aec66c3-c403-44b4-ac4d-fb8c8aa83c20'
    }
  )
  NestedModules         = @()
  FunctionsToExport     = @(
    'Test-NodePackageDirectory'
    'Resolve-NodePackageDirectory'
    'Invoke-Node'
    'Invoke-NodePackage'
    'Invoke-NodeExecutable'
    'Clear-NodeModuleCache'
    'Compare-NodeModule'
    'Step-NodePackageVersion'
    'Invoke-NodePackageScript'
    'Test-NodePackage'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'no'
    'n'
    'nx'
    'ncc'
    'npo'
    'nr'
    'nt'
  )
}
