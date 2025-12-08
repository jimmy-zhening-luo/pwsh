@{
  RootModule           = 'PSTool.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'c84491db-0b43-4dfc-80ea-890b16269a28'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    'Shell'
    'Git'
  )
  NestedModules        = @(
    'PSTool.Help'
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
