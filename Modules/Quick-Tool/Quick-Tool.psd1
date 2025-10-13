@{
  ModuleVersion     = "1.0.0.0"
  GUID              = "8984f438-f595-48c1-92a1-b893106bbfe2"
  NestedModules     = @(
    ".\Copy-Guid"
    ".\Get-YouTube"
  )
  FunctionsToExport = @(
    "Copy-Guid"
    "Get-YouTube"
    "Get-YouTubeAudio"
    "Get-YouTubeFormat"
  )
  AliasesToExport   = @(
    "guid"
    "yt"
    "yta"
    "ytf"
  )
}
