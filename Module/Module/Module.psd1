@{
  RootModule           = 'Module.dll'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '0d6f7f29-9175-410d-bd89-622ad5926f29'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @()
  CmdletsToExport      = @(
    'Clear-Line'
    'Set-Directory'
    'Stop-Task'
    'Edit-SystemPath'
    'Update-Windows'
    'Update-StoreApp'
    'Open-Url'
    'Test-Url'
    'Test-Host'
    'Copy-Guid'
    'ConvertTo-Hex'
    'Invoke-YouTubeDirectory'
    'Test-Cmdlet'
    'Start-Profile'
    'Start-History'
  )
  VariablesToExport    = @()
  AliasesToExport      = @(
    'cl'
    'clear'
    'c'
    'tkill'
    'path'
    'wu'
    'su'
    'o'
    'open'
    'tu'
    'tn'
    'gu'
    'guid'
    'hex'
    'yte'
    'test'
    'op'
    'oc'
  )
}
