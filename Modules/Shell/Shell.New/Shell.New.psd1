@{
  RootModule        = 'Shell.New.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '9a3324b2-207d-4635-a8d6-ac8261181fb1'
  NestedModules     = @(
    '.\Shell.New.Directory'
    '.\Shell.New.Junction'
  )
  FunctionsToExport = @(
    'New-Directory'
    'New-Junction'
  )
  AliasesToExport   = @(
    'touch'
    'mk'
    'mj'
  )
}
