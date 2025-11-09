@{
  RootModule        = "Edit-Item.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "1b1067fe-8dca-48e6-9734-78b3239579b2"
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
