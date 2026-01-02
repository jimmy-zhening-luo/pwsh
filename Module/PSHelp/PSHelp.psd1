@{
  RootModule           = 'PSHelp.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '2a24daf1-f891-46c4-8613-9943b6913573'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  RequiredModules      = @(
    @{
      ModuleName    = 'Core'
      ModuleVersion = '3.0.0'
      GUID          = 'a2075667-de18-47e9-9804-bbf47f23131f'
    }
  )
  FunctionsToExport    = @(
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
    'Get-TypeAccelerator'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'psk'
    'upman'
    'm'
    'man'
    'galc'
    'ty'
    'types'
  )
}
