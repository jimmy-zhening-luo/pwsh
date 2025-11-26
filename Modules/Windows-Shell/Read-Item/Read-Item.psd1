@{
  RootModule        = 'Read-Item.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '0af6fb12-d4f0-4836-8f8d-37bc9382c9ab'
  FunctionsToExport = @(
    'Read-Item'
    'Read-Sibling'
    'Read-Relative'
    'Read-Home'
    'Read-Code'
    'Read-Drive'
  )
  AliasesToExport   = @(
    'p'
    'p.'
    'p..'
    'p~'
    'pc'
    'p\'
    'p/'
  )
}
