@{
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '5aa4e6fd-3c16-4ee5-9019-8445bddd890c'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
  NestedModules        = @(
    @{
      ModuleName    = 'Code.Git'
      ModuleVersion = '3.0.0.0'
      GUID          = 'e76317a1-f5d7-42d8-90c7-87ac23270155'
    }
    @{
      ModuleName    = 'Code.Node'
      ModuleVersion = '3.0.0.0'
      GUID          = '886a95dd-cf5d-4016-8015-5f9a9345d66c'
    }
  )
  FunctionsToExport    = @(
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
    'Invoke-Node'
    'Invoke-NodePackage'
    'Invoke-NodeExecutable'
    'Clear-NodeModuleCache'
    'Compare-NodeModule'
    'Step-NodePackageVersion'
    'Invoke-NodePackageScript'
    'Test-NodePackage'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
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
    'nu'
    'nr'
    'nt'
  )
}
