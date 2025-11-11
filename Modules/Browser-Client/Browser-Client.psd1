@{
  ModuleVersion     = "1.0.0.0"
  GUID              = "ff158ab2-d35c-450c-8086-03480d1b580e"
  NestedModules     = @(
    ".\Test-Host"
    ".\Test-Url"
    ".\Open-Url"
  )
  FunctionsToExport = @(
    "Test-Host"
    "Test-Url"
    "Open-Url"
  )
  AliasesToExport   = @(
    "tn"
    "open"
    "go"
  )
}
