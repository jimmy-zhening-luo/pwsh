@{
  RootModule        = ".\Node-PackageManager.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "7e838ead-1871-47f2-a845-3ce3725b7781"
  NestedModules     = @(
    ".\Clear-PackageCache"
    ".\Compare-Package"
    ".\Invoke-Script"
  )
  FunctionsToExport = @(
    "Resolve-NodeProject"
    "Clear-PackageCache"
    "Compare-Package"
    "Invoke-Script"
  )
  AliasesToExport   = @(
    "npc"
    "npo"
    "nr"
  )
}
