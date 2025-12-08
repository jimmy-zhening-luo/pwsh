@{
  RootModule        = 'Quick.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '8984f438-f595-48c1-92a1-b893106bbfe2'
  PowerShellVersion = '7.5'
  NestedModules     = @(
    'Quick.YouTube'
  )
  FunctionsToExport = @(
    'Copy-Guid'
    'ConvertTo-Hex'
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
    'Invoke-YouTubeDirectory'
    'Invoke-YouTubeConfig'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'guid'
    'hex'
    'yt'
    'yta'
    'ytf'
    'yte'
    'ytc'
  )
}
