@{
  ModuleVersion     = '1.0.0.0'
  GUID              = '73ded6ab-d0a5-4d7c-a014-fa8b32a7714b'
  NestedModules     = @(
    '.\Store-App'
    '.\WinGet-App'
  )
  FunctionsToExport = @(
    'Update-StoreApp'
    'Add-WinGetApp'
    'Update-WinGetApp'
    'Remove-WinGetApp'
    'Find-WinGetApp'
  )
  AliasesToExport   = @(
    'su'
    'gapx'
    'remapx'
    'wget'
    'wga'
    'wgu'
    'wgr'
    'wgf'
  )
}
