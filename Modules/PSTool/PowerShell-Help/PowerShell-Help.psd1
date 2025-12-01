@{
  RootModule        = 'PowerShell-Help.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '2a24daf1-f891-46c4-8613-9943b6913573'
  RequiredModules   = @(
    'Browse'
  )
  FunctionsToExport = @(
    'Get-HelpOnline'
  )
  AliasesToExport   = @(
    'm'
    'upman'
  )
}
