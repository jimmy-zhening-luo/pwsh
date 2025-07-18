@{
  RootModule = "PowerShell-Utility.psm1"
  ModuleVersion = "1.0"
  GUID = "c84491db-0b43-4dfc-80ea-890b16269a28"
  NestedModules = @(
    ".\PowerShell-Debug"
  )
  FunctionsToExport = @(
    "Measure-PowerShellProfile"
    "Measure-PowerShellNoProfile"
  )
  AliasesToExport = @(
    "mc"
    "mcn"
  )
}
