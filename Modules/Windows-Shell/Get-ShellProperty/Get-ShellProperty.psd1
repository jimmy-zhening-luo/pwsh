@{
  RootModule        = 'Get-ShellProperty.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = 'c3bc46c2-3690-4860-b6d5-84806ca35f4b'
  NestedModules     = @(
    '.\Get-Size'
  )
  FunctionsToExport = @(
    'Get-Size'
  )
  AliasesToExport   = @(
    'split'
    'parent'
    'hash'
    'size'
  )
}
