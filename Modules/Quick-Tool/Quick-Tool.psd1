@{
  ModuleVersion     = '1.0.0.0'
  GUID              = '8984f438-f595-48c1-92a1-b893106bbfe2'
  NestedModules     = @(
    '.\Copy-Guid'
    '.\Convert-Number'
    '.\Convert-String'
    '.\Get-YouTube'
  )
  FunctionsToExport = @(
    'Copy-Guid'
    'ConvertTo-Hex'
    'ConvertTo-HexLower'
    'Format-Count'
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
  )
  AliasesToExport   = @(
    'guid'
    'hex'
    'hexl'
    'plural'
    'yt'
    'yta'
    'ytf'
  )
}
