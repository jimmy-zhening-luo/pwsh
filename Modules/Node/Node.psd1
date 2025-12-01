@{
  RootModule        = 'Node.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '7e838ead-1871-47f2-a845-3ce3725b7781'
  RequiredModules   = @(
    'Shell'
  )
  NestedModules     = @(
    'Clear-PackageCache'
    'Compare-Package'
    'Invoke-Script'
  )
  FunctionsToExport = @(
    'Resolve-NodeProject'
    'Clear-PackageCache'
    'Compare-Package'
    'Invoke-Script'
  )
  AliasesToExport   = @(
    'npc'
    'npo'
    'nr'
  )
}
