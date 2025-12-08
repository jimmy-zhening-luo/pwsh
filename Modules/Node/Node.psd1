@{
  RootModule           = 'Node.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '7e838ead-1871-47f2-a845-3ce3725b7781'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      Guid          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
    @{
      ModuleName    = 'Shell'
      ModuleVersion = '3.0.0'
      Guid          = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
    }
  )
  NestedModules        = @()
  FunctionsToExport    = @(
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
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'no'
    'n'
    'nx'
    'ncc'
    'npo'
    'nr'
    'nt'
  )
}
