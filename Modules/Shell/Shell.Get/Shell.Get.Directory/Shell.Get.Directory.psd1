@{
  RootModule        = 'Shell.Get.Directory.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '1c90175d-d43c-4e5e-9bd2-160c173da3e7'
  FunctionsToExport = @(
    'Get-Directory'
    'Get-DirectorySibling'
    'Get-DirectoryRelative'
    'Get-DirectoryHome'
    'Get-DirectoryCode'
    'Get-DirectoryDrive'
  )
  AliasesToExport   = @(
    'l'
    'l.'
    'l..'
    'l~'
    'lc'
    'l/'
  )
}
