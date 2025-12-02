@{
  RootModule        = 'PSTool.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = 'c84491db-0b43-4dfc-80ea-890b16269a28'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'PSTool.Help'
  )
  FunctionsToExport = @(
    'Invoke-PSHistory'
    'Invoke-PSProfile'
    'Update-PSProfile'
    'Update-PSLinter'
    'Measure-PSProfile'
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    '^'
    'pick'
    'k'
    'key'
    'n'
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
