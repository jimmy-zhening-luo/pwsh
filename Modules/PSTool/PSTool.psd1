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
      ModuleName    = 'Shell'
      ModuleVersion = '3.0.0'
      GUID          = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
    }
    @{
      ModuleName    = 'Git'
      ModuleVersion = '3.0.0'
      GUID          = '05b1300b-3c15-49d7-8033-a7edb1386042'
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
    'Update-PSLinter'
    'Measure-PSProfile'
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    '^'
    'to'
    'k'
    'key'
    'count'
    'z'
    'format'
    'oc'
    'op'
    'up'
    'mc'
    'upman'
    'm'
    'galc'
  )
}
