@{
  RootModule        = 'Pull-Repository.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '50d03f21-3ed8-4c85-acec-6b4d114efd31'
  FunctionsToExport = @(
    'Get-Repository'
    'Get-ChildRepository'
  )
  AliasesToExport   = @(
    'gitp'
    'ggp'
    'gpa'
  )
}
