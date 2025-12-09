@{
  RootModule            = 'Shell.psm1'
  ModuleVersion         = '3.0.0.0'
  CompatiblePSEditions  = @('Desktop', 'Core')
  GUID                  = 'e4d07654-6759-4a2f-8293-39df2b809ba7'
  Author                = 'Jimmy Zhening Luo'
  CompanyName           = 'Jimmy Zhening Luo'
  Copyright             = '(c) 2025 Jimmy Zhening Luo. All rights reserved.'
  PowerShellVersion     = '7.5'
  PowerShellHostName    = 'ConsoleHost'
  PowerShellHostVersion = '7.5'
  RequiredModules       = @(
    @{
      ModuleName    = 'PathArgumentCompleter'
      ModuleVersion = '3.0.0'
      GUID          = '4aec66c3-c403-44b4-ac4d-fb8c8aa83c20'
    }
    @{
      ModuleName    = 'GenericArgumentCompleter'
      ModuleVersion = '3.0.0'
      GUID          = 'ce7965e6-f9ef-42fb-aa4b-80eb542833de'
    }
  )
  NestedModules         = @(
    @{
      ModuleName    = 'Shell.Browse'
      ModuleVersion = '3.0.0.0'
      GUID          = '1e45c553-ea48-41c2-a7fc-89b5c36f30b1'
    }
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
      ModuleName    = 'Shell.Set'
      ModuleVersion = '3.0.0.0'
      GUID          = 'afed7375-abb6-4d62-9b11-fa07320220aa'
    }
    @{
      ModuleName    = 'Shell.Invoke'
      ModuleVersion = '3.0.0.0'
      GUID          = '130b56d3-229a-4ec1-be75-d31a615147c8'
    }
    @{
      ModuleName    = 'Shell.Remove'
      ModuleVersion = '3.0.0.0'
      GUID          = '1099153a-7f76-415c-8b8f-d18f5351d581'
    }
  )
  FunctionsToExport     = @(
    'Clear-Line'
    'Format-Path'
    'Trace-RelativePath'
    'Merge-RelativePath'
    'Test-Item'
    'Resolve-Item'
    'Test-Host'
    'Test-Url'
    'Open-Url'
    'New-Directory'
    'New-Junction'
    'Get-Size'
    'Get-Directory'
    'Get-DirectorySibling'
    'Get-DirectoryRelative'
    'Get-DirectoryHome'
    'Get-DirectoryCode'
    'Get-DirectoryDrive'
    'Get-File'
    'Get-FileSibling'
    'Get-FileRelative'
    'Get-FileHome'
    'Get-FileCode'
    'Get-FileDrive'
    'Set-Directory'
    'Set-DirectorySibling'
    'Set-DirectoryRelative'
    'Set-DirectoryHome'
    'Set-DirectoryCode'
    'Set-Drive'
    'Set-DriveD'
    'Invoke-Directory'
    'Invoke-DirectorySibling'
    'Invoke-DirectoryRelative'
    'Invoke-DirectoryHome'
    'Invoke-DirectoryCode'
    'Invoke-DirectoryDrive'
    'Invoke-Workspace'
    'Invoke-WorkspaceSibling'
    'Invoke-WorkspaceRelative'
    'Invoke-WorkspaceHome'
    'Invoke-WorkspaceCode'
    'Invoke-WorkspaceDrive'
    'Remove-Directory'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'cl'
    'tn'
    'tu'
    'go'
    'open'
    'touch'
    'mk'
    'mj'
    'split'
    'hash'
    'sz'
    'size'
    'l'
    'l.'
    'l..'
    'l~'
    'lc'
    'l/'
    'p'
    'p.'
    'p..'
    'p~'
    'pc'
    'p/'
    'c'
    'c.'
    'c..'
    'c~'
    'cc'
    'c/'
    'd/'
    'e'
    'e.'
    'e..'
    'e~'
    'ec'
    'e/'
    'i'
    'i.'
    'i..'
    'i~'
    'ic'
    'i/'
  )
}
