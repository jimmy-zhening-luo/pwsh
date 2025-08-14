@{
  RootModule        = "Reset-Repository.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "ba9a5388-7511-4093-828f-c889f540a2c1"
  FunctionsToExport = @(
    "Undo-Repository"
    "Restore-Repository"
  )
  AliasesToExport   = @(
    "gitr"
    "gr"
    "gitrp"
    "grp"
  )
}
