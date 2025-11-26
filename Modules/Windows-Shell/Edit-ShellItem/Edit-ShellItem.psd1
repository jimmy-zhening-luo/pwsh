@{
  RootModule        = 'Edit-ShellItem.psm1'
  ModuleVersion     = '1.0.0.0'
  GUID              = '58837453-25d1-453b-a77f-e37939368d68'
  RequiredModules   = @(
    'Argument-Completer'
  )
  FunctionsToExport = @(
    'Edit-ShellItem'
    'Edit-ShellSibling'
    'Edit-ShellRelative'
    'Edit-ShellHome'
    'Edit-ShellCode'
    'Edit-ShellDrive'
  )
  AliasesToExport   = @(
    'i'
    'i.'
    'i..'
    'i~'
    'ic'
    'i\'
    'i/'
  )
}
