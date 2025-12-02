@{
  RootModule        = 'WindowsSystem.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'b047ad82-dcbf-48cc-876a-78c6334900af'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'WindowsSystem.Run'
    'WindowsSystem.Setting'
    'WindowsSystem.Task'
    'WindowsSystem.App'
  )
  FunctionsToExport = @(
    'Invoke-CommandPrompt'
    'Update-Windows'
    'Edit-Path'
    'Stop-Task'
    'Update-StoreApp'
    'Add-WinGetApp'
    'Update-WinGetApp'
    'Remove-WinGetApp'
    'Find-WinGetApp'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'restart'
    'wu'
    'path'
    'sesv'
    'remsv'
    'tkill'
    'su'
    'gapx'
    'remapx'
    'wget'
    'wga'
    'wgu'
    'wgr'
    'wgf'
  )
}
