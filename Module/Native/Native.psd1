@{
  RootModule           = 'Native.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '7da18ef1-68e8-4a56-8365-bcb500357072'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  FunctionsToExport    = @(
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'clb'
    'k'
    'key'
    'psk'
    'verb'
    'ct'
    'count'
    'touch'
    'remsv'
    'restart'
    '^'
    's'
    'sesv'
    'z'
    'upman'
    'wg'
    'wga'
    'wgf'
    'wgr'
  )
}
