@{
  ModuleVersion     = '3.0.0.0'
  GUID              = '73ded6ab-d0a5-4d7c-a014-fa8b32a7714b'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'WindowsSystem.App.Store'
    'WindowsSystem.App.WinGet'
  )
  FunctionsToExport = @(
    'Update-StoreApp'
    'Update-WinGetApp'
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'su'
    'gapx'
    'remapx'
    'wget'
    'wgu'
    'wga'
    'wgf'
    'wgr'
  )
}
