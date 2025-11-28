@{
  RootModule        = 'WinGet-App.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = 'dad9ca1b-2892-425d-8033-3fdfe61be7cd'
  NestedModules     = @(
    '.\Add-WinGetApp'
    '.\Update-WinGetApp'
    '.\Remove-WinGetApp'
    '.\Find-WinGetApp'
  )
  FunctionsToExport = @(
    'Add-WinGetApp'
    'Update-WinGetApp'
    'Remove-WinGetApp'
    'Find-WinGetApp'
  )
  AliasesToExport   = @(
    'wget'
    'wga'
    'wgu'
    'wgr'
    'wgf'
  )
}
