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
      ModuleName    = 'Module'
      ModuleVersion = '3.0.0'
      GUID          = '0d6f7f29-9175-410d-bd89-622ad5926f29'
    }
  )
  FunctionsToExport    = @(
    'Get-HelpOnline'
    'Get-CommandAlias'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'm'
    'man'
    'galc'
  )
}
