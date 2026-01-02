@{
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'f6405b67-31f3-4633-ba81-feb8364d2395'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  RequiredModules      = @(
    @{
      ModuleName    = 'Core'
      ModuleVersion = '3.0.0'
      GUID          = 'a2075667-de18-47e9-9804-bbf47f23131f'
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
    'yt'
    'yta'
    'ytf'
    'yte'
    'ytc'
  )
}
