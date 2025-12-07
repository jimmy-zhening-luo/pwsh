@{
  RootModule        = 'Node.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '7e838ead-1871-47f2-a845-3ce3725b7781'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
    'GenericArgumentCompleter'
  )
  FunctionsToExport = @(
    'Resolve-NodePackage'
    'Invoke-Node'
    'Invoke-NodeExecutable'
    'Clear-NodeModuleCache'
    'Compare-NodeModule'
    'Step-NodePackageVersion'
    'Invoke-NodePackageScript'
    'Test-NodePackage'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'n'
    'nx'
    'ncc'
    'npo'
    'nr'
    'nt'
  )
}
