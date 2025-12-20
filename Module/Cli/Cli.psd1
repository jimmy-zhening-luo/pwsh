@{
  RootModule           = 'Cli.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'f6405b67-31f3-4633-ba81-feb8364d2395'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  RequiredModules      = @(
    @{
      ModuleName    = 'Browse'
      ModuleVersion = '3.0.0'
      GUID          = '5a80488d-6a5a-4f24-b3f1-ff9d275ab8b5'
    }
    @{
      ModuleName    = 'Shell'
      ModuleVersion = '3.0.0'
      GUID          = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
    }
  )
  NestedModules        = @(
    @{
      ModuleName    = 'Cli.YouTube'
      ModuleVersion = '3.0.0'
      GUID          = 'e5eeffaf-374b-419e-a610-68015eaee174'
    }
  )
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
    'dotnet'
    'yt'
    'yta'
    'ytf'
    'yte'
    'ytc'
  )
}
