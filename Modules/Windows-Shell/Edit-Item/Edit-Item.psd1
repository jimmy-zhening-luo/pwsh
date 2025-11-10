@{
  RootModule        = "Edit-Item.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "58837453-25d1-453b-a77f-e37939368d68"
  FunctionsToExport = @(
    "Edit-Item"
    "Edit-Sibling"
    "Edit-Relative"
    "Edit-Home"
    "Edit-Code"
    "Edit-Drive"
  )
  AliasesToExport   = @(
    "i"
    "i."
    "i.."
    "i~"
    "ic"
    "i\"
    "i/"
  )
}
