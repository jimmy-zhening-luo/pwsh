@{
  RootModule        = 'Shell.Get.File.psm1'
  ModuleVersion     = '3.0.0.0'
  GUID              = '6212672b-6168-4bcb-a3bf-38291446571a'
  PowerShellVersion = '7.5'
  FunctionsToExport = @(
    'Get-File'
    'Get-FileSibling'
    'Get-FileRelative'
    'Get-FileHome'
    'Get-FileCode'
    'Get-FileDrive'
  )
  CmdletsToExport   = @()
  VariablesToExport = @()
  AliasesToExport   = @(
    'p'
    'p.'
    'p..'
    'p~'
    'pc'
    'p/'
  )
}
