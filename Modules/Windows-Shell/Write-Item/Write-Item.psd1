@{
  RootModule        = 'Write-Item.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '3c8c5daf-6b0c-438c-b3d4-53e7306569ba'
  RequiredModules   = @(
    'Argument-Completer'
  )
  FunctionsToExport = @(
    'Write-Item'
    'Write-Sibling'
    'Write-Relative'
    'Write-Home'
    'Write-Code'
    'Write-Drive'
  )
  AliasesToExport   = @(
    'w'
    'w.'
    'w..'
    'w~'
    'wc'
    'w\'
    'w/'
  )
}
