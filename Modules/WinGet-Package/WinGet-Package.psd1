@{
  RootModule        = "WinGet-Package.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "3a5b4926-a508-40f6-8df1-e8415625ac12"
  NestedModules     = @(
    ".\Add-WinGetPackage"
    ".\Update-WinGetPackage"
    ".\Remove-WinGetPackage"
    ".\Find-WinGetPackage"
  )
  FunctionsToExport = @(
    "Add-WinGetPackage"
    "Update-WinGetPackage"
    "Remove-WinGetPackage"
    "Find-WinGetPackage"
  )
  AliasesToExport   = @(
    "gapx"
    "remapx"
    "wga"
    "wgu"
    "wgr"
    "wgf"
  )
}
