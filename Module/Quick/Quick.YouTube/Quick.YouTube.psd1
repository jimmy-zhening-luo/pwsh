@{
  RootModule            = 'Quick.YouTube.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = 'bc355ec2-c3a4-4e99-abe1-4e9e2aeb2635'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredModules       = @(
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
  FunctionsToExport     = @(
    'Get-YouTube'
    'Get-YouTubeAudio'
    'Get-YouTubeFormat'
    'Invoke-YouTubeDirectory'
    'Invoke-YouTubeConfig'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'yt'
    'yta'
    'ytf'
    'yte'
    'ytc'
  )
}
