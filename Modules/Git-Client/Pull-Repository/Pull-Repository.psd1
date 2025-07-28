@{
  RootModule        = "Pull-Repository.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "50d03f21-3ed8-4c85-acec-6b4d114efd31"
  FunctionsToExport = @(
    "Get-Repository"
    "Get-ChildRepository"
  )
  AliasesToExport   = @(
    "gitp"
    "gpa"
  )
}
