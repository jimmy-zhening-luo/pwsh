@{
  RootModule        = 'Git.Pull.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '50d03f21-3ed8-4c85-acec-6b4d114efd31'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Get-Repository'
    'Get-ChildRepository'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'ggp'
    'gpa'
  )
}
