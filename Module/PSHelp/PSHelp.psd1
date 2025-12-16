@{
  RootModule           = 'PSHelp.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '2a24daf1-f891-46c4-8613-9943b6913573'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    @{
      ModuleName    = 'Completer'
      ModuleVersion = '3.0.0'
      GUID          = '95f00487-efc3-43e8-adcb-c539f28f0058'
    }
    @{
      ModuleName    = 'Browse'
      ModuleVersion = '3.0.0'
      GUID          = '5a80488d-6a5a-4f24-b3f1-ff9d275ab8b5'
    }
  )
  FunctionsToExport    = @(
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'psk'
    'upman'
    'm'
    'galc'
  )
}
