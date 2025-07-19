@{
  RootModule        = "PowerShell-Debug.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "d97743fb-8ecf-4420-9862-2704499e99ef"
  FunctionsToExport = @(
    "Measure-Profile"
    "Measure-NoProfile"
  )
  AliasesToExport   = @(
    "mc"
    "mn"
  )
}
