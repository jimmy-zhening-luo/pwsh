@{
  ModuleVersion     = "1.0.0.0"
  GUID              = "8984f438-f595-48c1-92a1-b893106bbfe2"
  NestedModules     = @(
    ".\Copy-Guid"
    ".\Edit-String"
    ".\Get-YouTube"
  )
  FunctionsToExport = @(
    "Copy-Guid"
    "Format-Count"
    "Get-YouTube"
    "Get-YouTubeAudio"
    "Get-YouTubeFormat"
  )
  AliasesToExport   = @(
    "guid"
    "plural"
    "yt"
    "yta"
    "ytf"
  )
}
