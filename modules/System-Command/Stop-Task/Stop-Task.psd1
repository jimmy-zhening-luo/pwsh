@{
  RootModule        = "Stop-Task.psm1"
  ModuleVersion     = "1.0"
  GUID              = "2a3c21a6-d75f-4256-a385-5888fce17a64"
  FunctionsToExport = @(
    "Stop-Task"
  )
  AliasesToExport   = @(
    "tkill"
    "tkillx"
  )
}
