@{
  RootModule        = 'Commit-Repository.psm1'
  ModuleVersion     = '3.0.0.0'
  PowerShellVersion = '7.5'
  GUID              = '91560f32-be66-471e-810f-d366a20781d3'
  FunctionsToExport = @(
    'Write-Repository'
  )
  AliasesToExport   = @(
    'gitm'
    'ggm'
  )
}
