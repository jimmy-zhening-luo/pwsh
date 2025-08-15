@{
  RootModule        = "Get-Folder.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "b04dc21c-3d93-42fb-bafc-3ab15860ad95"
  FunctionsToExport = @(
    "Get-Sibling"
    "Get-Relative"
    "Get-Home"
    "Get-Drive"
  )
  AliasesToExport   = @(
    "l"
    "l."
    "l.."
    "l~"
    "l\"
    "l/"
  )
}
