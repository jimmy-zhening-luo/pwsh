@{
  RootModule           = 'Quick.YouTube.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'bc355ec2-c3a4-4e99-abe1-4e9e2aeb2635'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    @{
      ModuleName    = 'Shell'
      ModuleVersion = '3.0.0'
      Guid          = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
    }
  )
  NestedModules        = @()
  FunctionsToExport    = @(
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
    'Invoke-YouTubeDirectory'
    'Invoke-YouTubeConfig'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'yt'
    'yta'
    'ytf'
    'yte'
    'ytc'
  )
}
