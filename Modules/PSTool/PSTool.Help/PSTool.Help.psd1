@{
  RootModule            = 'PSTool.Help.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = '2a24daf1-f891-46c4-8613-9943b6913573'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredModules       = @()
  NestedModules         = @()
  FunctionsToExport     = @(
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'upman'
    'm'
    'galc'
  )
  ModuleList            = @()
  FileList              = @(
    'PSTool.Help.psd1'
    'PSTool.Help.psm1'
    'PSHelp.psd1'
  )
}
