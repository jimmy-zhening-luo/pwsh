@{
  RootModule        = 'Node.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '7e838ead-1871-47f2-a845-3ce3725b7781'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
  )
  FunctionsToExport = @(
    'Resolve-NodeProject'
    'Clear-PackageCache'
    'Compare-Package'
    'Invoke-Script'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'npc'
    'npo'
    'nr'
  )
}
