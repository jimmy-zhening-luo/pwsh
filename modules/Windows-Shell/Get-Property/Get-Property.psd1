@{
  RootModule        = "Get-Property.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "05e54ed5-5b35-418b-98df-59a727e128f1"
  FunctionsToExport = @(
    "Get-Parent"
    "Get-FileSize"
  )
  AliasesToExport   = @(
    "parent"
    "size"
  )
}
