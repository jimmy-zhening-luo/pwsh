@{
  RootModule        = 'Invoke-Directory.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '910e78d1-480c-449f-ae27-449dbd1a7910'
  FunctionsToExport = @(
    'Invoke-Directory'
    'Invoke-Sibling'
    'Invoke-Relative'
    'Invoke-Home'
    'Invoke-Code'
    'Invoke-Drive'
  )
  AliasesToExport   = @(
    'e'
    'e.'
    'e..'
    'e~'
    'ec'
    'e\'
    'e/'
  )
}
