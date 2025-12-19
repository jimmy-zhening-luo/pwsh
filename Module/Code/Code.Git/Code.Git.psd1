@{
  RootModule           = 'Code.Git.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'e76317a1-f5d7-42d8-90c7-87ac23270155'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.4'
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
