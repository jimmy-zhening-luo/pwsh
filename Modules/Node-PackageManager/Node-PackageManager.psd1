@{
  RootModule        = ".\Node-PackageManager.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "7e838ead-1871-47f2-a845-3ce3725b7781"
  NestedModules     = @(
    ".\Compare-Package"
  )
  FunctionsToExport = @(
    "Compare-Package"
  )
  AliasesToExport   = @(
    "npo"
  )
}
