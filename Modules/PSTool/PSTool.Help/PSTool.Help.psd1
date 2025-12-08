@{
  RootModule        = 'PSTool.Help.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '2a24daf1-f891-46c4-8613-9943b6913573'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Shell'
  )
  FunctionsToExport = @(
    'Get-HelpOnline'
    'Get-CommandAlias'
    'Get-VerbList'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'upman'
    'm'
    'galc'
  )
}
