@{
  RootModule        = "PowerShell-Profile.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "371a9d71-75b4-4e2d-94f5-8c7d832d4f20"
  FunctionsToExport = @(
    "Edit-Profile"
    "Sync-Profile"
  )
  AliasesToExport   = @(
    "op"
    "up"
  )
}
