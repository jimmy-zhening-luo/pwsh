@{
  RootModule           = 'PSTool.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'c84491db-0b43-4dfc-80ea-890b16269a28'
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
    @{
      ModuleName    = 'Shell'
      ModuleVersion = '3.0.0'
      GUID          = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
    }
    @{
      ModuleName    = 'WindowsSystem'
      ModuleVersion = '3.0.0'
      GUID          = 'b047ad82-dcbf-48cc-876a-78c6334900af'
    }
  )
  NestedModules        = @(
    @{
      ModuleName    = 'PSTool.Help'
      ModuleVersion = '3.0.0'
      GUID          = '2a24daf1-f891-46c4-8613-9943b6913573'
    }
  )
  FunctionsToExport    = @(
    'Invoke-PSHistory'
    'Invoke-PSProfile'
    'Update-PSProfile'
    'Measure-PSProfile'
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'oc'
    'op'
    'up'
    'mc'
    'upman'
    'psk'
    'm'
    'galc'
  )
}
