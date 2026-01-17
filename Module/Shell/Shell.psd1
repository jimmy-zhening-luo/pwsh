@{
  ModuleVersion        = '3.0.0.0'
  CompatiblePSEditions = @('Desktop', 'Core')
  GUID                 = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
  Author               = 'Jimmy Zhening Luo'
  CompanyName          = 'Jimmy Zhening Luo'
  Copyright            = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion    = '7.5'
  NestedModules        = @(
    @{
      ModuleName    = 'Shell.New'
      ModuleVersion = '3.0.0.0'
      GUID          = '9a3324b2-207d-4635-a8d6-ac8261181fb1'
    }
    @{
      ModuleName    = 'Shell.Get'
      ModuleVersion = '3.0.0.0'
      GUID          = '83fda875-8f8c-4bce-a5da-e840d832378d'
    }
    @{
      ModuleName    = 'Shell.Start'
      ModuleVersion = '3.0.0.0'
      GUID          = '043fee19-632c-4e0a-ac64-31b9fe02be00'
    }
    @{
      ModuleName    = 'Shell.Remove'
      ModuleVersion = '3.0.0.0'
      GUID          = '1099153a-7f76-415c-8b8f-d18f5351d581'
    }
  )
  FunctionsToExport    = @(
    'New-Directory'
    'New-Junction'
    'Get-Size'
    'Get-File'
    'Get-FileSibling'
    'Get-FileRelative'
    'Get-FileHome'
    'Get-FileCode'
    'Get-FileDrive'
    'Start-Explorer'
    'Start-ExplorerSibling'
    'Start-ExplorerRelative'
    'Start-ExplorerHome'
    'Start-ExplorerCode'
    'Start-ExplorerDrive'
    'Start-Workspace'
    'Start-WorkspaceSibling'
    'Start-WorkspaceRelative'
    'Start-WorkspaceHome'
    'Start-WorkspaceCode'
    'Start-WorkspaceDrive'
    'Remove-Directory'
  )
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @(
    'touch'
    'mk'
    'mj'
    'split'
    'hash'
    'size'
    'sz'
    'p'
    'p.'
    'p..'
    'ph'
    'pc'
    'p/'
    'e'
    'e.'
    'e..'
    'eh'
    'ec'
    'e/'
    'i'
    'i.'
    'i..'
    'ih'
    'ic'
    'i/'
    'rd'
  )
}
