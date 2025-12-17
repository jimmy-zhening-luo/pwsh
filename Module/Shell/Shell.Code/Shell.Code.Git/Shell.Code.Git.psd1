@{
  RootModule           = 'Shell.Code.Git.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '26c82360-9443-447e-8436-b1afe0e5a086'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
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
  )
}
