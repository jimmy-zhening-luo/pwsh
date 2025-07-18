@{
  RootModule        = "Get-FolderChildItem.psm1"
  ModuleVersion     = "1.0"
  GUID              = "69b314e8-5567-4986-995d-3ccbb2bc40a9"
  FunctionsToExport = @(
    "Get-SiblingItem"
    "Get-HomeItem"
  )
  AliasesToExport   = @(
    "l."
    "l.."
    "ls."
    "ls.."
    "l~"
    "ls~"
  )
}
