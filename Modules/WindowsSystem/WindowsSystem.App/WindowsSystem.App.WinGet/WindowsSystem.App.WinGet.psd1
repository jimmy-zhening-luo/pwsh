@{
  RootModule        = 'WindowsSystem.App.WinGet.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'dad9ca1b-2892-425d-8033-3fdfe61be7cd'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Update-WinGetApp'
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'wget'
    'wgu'
    'wga'
    'wgf'
    'wgr'
  )
}
