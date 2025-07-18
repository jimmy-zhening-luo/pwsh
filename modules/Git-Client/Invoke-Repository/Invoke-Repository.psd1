@{
  RootModule = "Invoke-Repository.psm1"
  ModuleVersion = "1.0"
  GUID = "366993a2-f8cc-425b-8aca-7f92de26ff16"
  NestedModules = @(
    ".\Resolve-Repository"
  )
  FunctionsToExport = @(
    "Invoke-Repository"
    "Resolve-Repository"
  )
  AliasesToExport = @(
    "gitc"
  )
}
