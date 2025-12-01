@{
  RootModule        = 'Shell.Get.Property.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = 'c3bc46c2-3690-4860-b6d5-84806ca35f4b'
  NestedModules     = @(
    '.\Shell.Get.Property.Size'
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
