@{
  RootModule        = "Update-System.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "b0c68601-a772-401b-b0c2-63629bfe32f2"
  FunctionsToExport = @(
    "Update-Windows"
    "Update-StoreApp"
  )
  AliasesToExport   = @(
    "wu"
    "su"
  )
}
