@{
  RootModule        = "Get-Directory.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "b04dc21c-3d93-42fb-bafc-3ab15860ad95"
  FunctionsToExport = @(
    "Get-Directory"
    "Get-Sibling"
    "Get-Relative"
    "Get-Home"
    "Get-Code"
    "Get-Drive"
  )
  AliasesToExport   = @(
    "l"
    "l."
    "l.."
    "l~"
    "lc"
    "l\"
    "l/"
  )
}
