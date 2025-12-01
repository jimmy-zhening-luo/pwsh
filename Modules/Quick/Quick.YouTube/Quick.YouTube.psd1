@{
  RootModule        = 'Quick.YouTube.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '06614306-7243-4ca6-8682-826c4fb31b11'
  RequiredModules   = @(
    'Browse'
  )
  FunctionsToExport = @(
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
  )
  AliasesToExport   = @(
    'yt'
    'yta'
    'ytf'
  )
}
