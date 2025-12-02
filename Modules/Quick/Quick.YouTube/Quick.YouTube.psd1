@{
  RootModule        = 'Quick.YouTube.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '06614306-7243-4ca6-8682-826c4fb31b11'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'Browse'
  )
  FunctionsToExport = @(
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'yt'
    'yta'
    'ytf'
  )
}
