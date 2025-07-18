@{
  RootModule        = "Invoke-Folder.psm1"
  ModuleVersion     = "1.0"
  GUID              = "910e78d1-480c-449f-ae27-449dbd1a7910"
  FunctionsToExport = @(
    "Invoke-Folder"
    "Invoke-Parent"
    "Invoke-Home"
    "Invoke-Drive"
  )
  AliasesToExport   = @(
    "explore"
    "e"
    "e."
    "e.."
    "e~"
    "e\"
    "e/"
  )
}
