@{
  ModuleVersion     = '1.0.0.0'
  GUID              = 'c84491db-0b43-4dfc-80ea-890b16269a28'
  NestedModules     = @(
    '.\PowerShell-Object'
    '.\PowerShell-Alias'
    '.\PowerShell-Help'
    '.\PowerShell-Verb'
    '.\PowerShell-History'
    '.\PowerShell-Profile'
    '.\PowerShell-Debug'
    '.\Lint-PowerShell'
  )
  FunctionsToExport = @(
    'Get-CommandAlias'
    'Get-HelpOnline'
    'Get-VerbList'
    'Invoke-PSHistory'
    'Invoke-PSProfile'
    'Update-PSProfile'
    'Measure-PSProfile'
    'Update-PSLinter'
  )
  AliasesToExport   = @(
    'k'
    'key'
    'keys'
    '^'
    'pick'
    'z'
    'n'
    'count'
    'format'
    'galc'
    'm'
    'upman'
    'oc'
    'op'
    'up'
    'mc'
  )
}
