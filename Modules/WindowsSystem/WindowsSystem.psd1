@{
  RootModule           = 'WindowsSystem.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'b047ad82-dcbf-48cc-876a-78c6334900af'
  PowerShellVersion    = '7.5'
  NestedModules        = @(
    @{
      ModuleName    = 'WindowsSystem.Update'
      ModuleVersion = '3.0.0'
      Guid          = '73ded6ab-d0a5-4d7c-a014-fa8b32a7714b'
    }
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
