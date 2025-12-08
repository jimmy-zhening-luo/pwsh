@{
  RootModule           = 'WindowsSystem.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'b047ad82-dcbf-48cc-876a-78c6334900af'
  PowerShellVersion    = '7.5'
  NestedModules        = @(
    'WindowsSystem.App'
  )
  FunctionsToExport    = @(
    'Invoke-CommandPrompt'
    'Update-Windows'
    'Edit-Path'
    'Stop-Task'
    'Update-StoreApp'
    'Update-WinGetApp'
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
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
    'wgu'
    'wga'
    'wgf'
    'wgr'
  )
}
