@{
  RootModule        = 'Convert-Number.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '3dd73a23-aa46-47f1-aff4-715e05936624'
  FunctionsToExport = @(
    'ConvertTo-Hex'
    'ConvertTo-HexLower'
  )
  AliasesToExport   = @(
    'hex'
    'hexl'
  )
}
