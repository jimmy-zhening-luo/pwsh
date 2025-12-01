@{
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '8984f438-f595-48c1-92a1-b893106bbfe2'
  NestedModules     = @(
    'Copy-Guid'
    'Convert-Number'
    'Get-YouTube'
  )
  FunctionsToExport = @(
    'Copy-Guid'
    'ConvertTo-Hex'
    'ConvertTo-HexLower'
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
  )
  AliasesToExport   = @(
    'guid'
    'hex'
    'hexl'
    'yt'
    'yta'
    'ytf'
  )
}
