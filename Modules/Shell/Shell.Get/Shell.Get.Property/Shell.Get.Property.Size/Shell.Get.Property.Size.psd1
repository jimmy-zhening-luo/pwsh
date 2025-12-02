@{
  RootModule        = 'Shell.Get.Property.Size.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '5c0ba366-63f2-41ad-910f-6fad8904a75f'
  PowerShellVersion = '7.5'
  RequiredModules   = @(
    'GenericArgumentCompleter'
  )
  FunctionsToExport = @(
    'Get-Size'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'size'
  )
}
