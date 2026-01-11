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
    'Stop-Task'
    'Edit-SystemPath'
    'Update-Windows'
    'Update-StoreApp'
    'Clear-Line'
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
    'tkill'
    'path'
    'wu'
    'su'
    'cl'
    'clear'
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
