@{
  RootModule           = 'Git.psm1'
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = '05b1300b-3c15-49d7-8033-a7edb1386042'
  PowerShellVersion    = '7.5'
  RequiredModules      = @(
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      Guid          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
    @{
      ModuleName    = 'Shell'
      ModuleVersion = '3.0.0'
      Guid          = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
    }
  )
  FunctionsToExport    = @(
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
