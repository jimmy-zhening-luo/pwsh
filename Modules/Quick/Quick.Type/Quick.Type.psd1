@{
  RootModule        = 'Quick.Type.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '8da8891d-38ff-45cd-8cfb-d781ee8ba7bc'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Copy-Guid'
    'ConvertTo-Hex'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'guid'
    'hex'
  )
}
