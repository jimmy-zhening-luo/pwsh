@{
  RootModule        = "Set-Folder.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "b6522962-e911-4dc4-bfbe-0181264ad7d2"
  FunctionsToExport = @(
    "Set-Code"
    "Set-Sibling"
    "Set-Relative"
    "Set-Home"
    "Set-Drive"
    "Set-DriveD"
  )
  AliasesToExport   = @(
    "c"
    "c."
    "c.."
    "c~"
    "c\"
    "c/"
    "cc"
    "d\"
    "d/"
  )
}
