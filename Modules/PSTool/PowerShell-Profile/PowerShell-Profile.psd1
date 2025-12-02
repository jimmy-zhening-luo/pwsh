@{
  RootModule        = 'PowerShell-Profile.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '371a9d71-75b4-4e2d-94f5-8c7d832d4f20'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
    'Git'
  )
  FunctionsToExport = @(
    'Invoke-PSProfile'
    'Update-PSProfile'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'op'
    'up'
  )
}
