@{
  RootModule = ".\Git-Client.psm1"
  ModuleVersion = "1.0"
  GUID = "d8a43de0-682d-42d5-8333-bfbc80d84ac4"
  RequiredModules = @(
    ".\Invoke-Repository"
  )
  NestedModules = @(
    ".\Verbs\Add-Repository"
  )
  AliasesToExport = @(
    "gita"
  )
}
