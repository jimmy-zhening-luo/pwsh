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
    'Update-WinGetApp'
    'Add-WinGetApp'
    'Find-WinGetApp'
    'Remove-WinGetApp'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
New-Alias ^ Select-Object
New-Alias s Select-Object
New-Alias sel Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias keys Get-Member
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias tab Format-Table
New-Alias table Format-Table
New-Alias format Format-Table

    'restart'
    'sesv'
    'remsv'
    'gapx'
    'remapx'
    'wget'
    'wgu'
    'wga'
    'wgf'
    'wgr'
  )
}
