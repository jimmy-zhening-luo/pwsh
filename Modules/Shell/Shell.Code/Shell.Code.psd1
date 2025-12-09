@{
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = 'a5751c41-5026-444f-8c2a-f13a2ac354f2'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  NestedModules         = @(
    @{
      ModuleName    = 'Shell.Code.Git'
      ModuleVersion = '3.0.0.0'
      GUID          = '26c82360-9443-447e-8436-b1afe0e5a086'
    }
    @{
      ModuleName    = 'Shell.Code.Node'
      ModuleVersion = '3.0.0.0'
      GUID          = '47836d9e-49bd-4405-bdc6-1900f3108d10'
    }
  )
  FunctionsToExport     = @(
    'Resolve-GitRepository'
    'Invoke-GitRepository'
    'Measure-GitRepository'
    'Import-GitRepository'
    'Get-GitRepository'
    'Get-ChildGitRepository'
    'Add-GitRepository'
    'Write-GitRepository'
    'Push-GitRepository'
    'Reset-GitRepository'
    'Restore-GitRepository'
    'Test-NodePackageDirectory'
    'Resolve-NodePackageDirectory'
    'Invoke-Node'
    'Invoke-NodePackage'
    'Invoke-NodeExecutable'
    'Clear-NodeModuleCache'
    'Compare-NodeModule'
    'Step-NodePackageVersion'
    'Invoke-NodePackageScript'
    'Test-NodePackage'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'g'
    'gg'
    'gitcl'
    'gpp'
    'ga'
    'gs'
    'gr'
    'grp'
    'no'
    'n'
    'nx'
    'ncc'
    'npo'
    'nr'
    'nt'
  )
}
