@{
  RootModule           = 'Quick.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '8984f438-f595-48c1-92a1-b893106bbfe2'
  PowerShellVersion    = '7.5'
  RequiredModules      = @()
  NestedModules        = @(
    @{
      ModuleName    = 'Quick.YouTube'
      ModuleVersion = '3.0.0'
      Guid          = 'bc355ec2-c3a4-4e99-abe1-4e9e2aeb2635'
    }
  )
  FunctionsToExport    = @(
    'Copy-Guid'
    'ConvertTo-Hex'
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
    'Invoke-YouTubeDirectory'
    'Invoke-YouTubeConfig'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'guid'
    'hex'
    'yt'
    'yta'
    'ytf'
    'yte'
    'ytc'
  )
}
