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
  )
  NestedModules         = @(
    @{
      ModuleName    = 'Shell.New'
      ModuleVersion = '3.0.0.0'
      GUID          = '9a3324b2-207d-4635-a8d6-ac8261181fb1'
    }
    @{
      ModuleName    = 'Shell.Clear'
      ModuleVersion = '3.0.0.0'
      GUID          = '7b39ad83-781d-49f2-a378-f913d983b1a6'
    }
    @{
      ModuleName    = 'Shell.Get'
      ModuleVersion = '3.0.0.0'
      GUID          = '83fda875-8f8c-4bce-a5da-e840d832378d'
    }
    @{
      ModuleName    = 'Shell.Set'
      ModuleVersion = '3.0.0.0'
      GUID          = 'e740b507-c756-4684-8565-8c579344e454'
    }
    @{
      ModuleName    = 'Shell.Invoke'
      ModuleVersion = '3.0.0.0'
      GUID          = '130b56d3-229a-4ec1-be75-d31a615147c8'
    }
    @{
      ModuleName    = 'Shell.Browse'
      ModuleVersion = '3.0.0.0'
      GUID          = '1e45c553-ea48-41c2-a7fc-89b5c36f30b1'
    }
  )
  FunctionsToExport     = @(
    'Format-Path'
    'Trace-RelativePath'
    'Merge-RelativePath'
    'Test-Item'
    'Resolve-Item'
    'New-Directory'
    'New-Junction'
    'Clear-Line'
    'Remove-Directory'
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
    'Test-Host'
    'Test-Url'
    'Open-Url'
  )
  CmdletsToExport       = @()
  VariablesToExport     = @()
  AliasesToExport       = @(
    'touch'
    'mk'
    'mj'
    'cl'
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
    'tn'
    'tu'
    'go'
    'open'
  )
  ModuleList            = @(
    @{
      ModuleName    = 'Shell.New'
      ModuleVersion = '3.0.0.0'
      GUID          = '9a3324b2-207d-4635-a8d6-ac8261181fb1'
    }
    @{
      ModuleName    = 'Shell.Clear'
      ModuleVersion = '3.0.0.0'
      GUID          = '7b39ad83-781d-49f2-a378-f913d983b1a6'
    }
    @{
      ModuleName    = 'Shell.Get'
      ModuleVersion = '3.0.0.0'
      GUID          = '83fda875-8f8c-4bce-a5da-e840d832378d'
    }
    @{
      ModuleName    = 'Shell.Set'
      ModuleVersion = '3.0.0.0'
      GUID          = 'e740b507-c756-4684-8565-8c579344e454'
    }
    @{
      ModuleName    = 'Shell.Invoke'
      ModuleVersion = '3.0.0.0'
      GUID          = '130b56d3-229a-4ec1-be75-d31a615147c8'
    }
    @{
      ModuleName    = 'Shell.Browse'
      ModuleVersion = '3.0.0.0'
      GUID          = '1e45c553-ea48-41c2-a7fc-89b5c36f30b1'
    }
  )
  FileList              = @(
    'Shell.psd1'
    'Shell.psm1'
    'Shell.New\Shell.New.psd1'
    'Shell.New\Shell.New.psm1'
    'Shell.Clear\Shell.Clear.psd1'
    'Shell.Clear\Shell.Clear.psm1'
    'Shell.Get\Shell.Get.psd1'
    'Shell.Get\Shell.Get.psm1'
    'Shell.Get\Shell.Get.Directory\Shell.Get.Directory.psd1'
    'Shell.Get\Shell.Get.Directory\Shell.Get.Directory.psm1'
    'Shell.Get\Shell.Get.File\Shell.Get.File.psd1'
    'Shell.Get\Shell.Get.File\Shell.Get.File.psm1'
    'Shell.Set\Shell.Set.psd1'
    'Shell.Set\Shell.Set.Directory\Shell.Set.Directory.psd1'
    'Shell.Set\Shell.Set.Directory\Shell.Set.Directory.psm1'
    'Shell.Invoke\Shell.Invoke.psd1'
    'Shell.Invoke\Shell.Invoke.Directory\Shell.Invoke.Directory.psd1'
    'Shell.Invoke\Shell.Invoke.Directory\Shell.Invoke.Directory.psm1'
    'Shell.Invoke\Shell.Invoke.Workspace\Shell.Invoke.Workspace.psd1'
    'Shell.Invoke\Shell.Invoke.Workspace\Shell.Invoke.Workspace.psm1'
    'Shell.Browse\Shell.Browse.psd1'
    'Shell.Browse\Shell.Browse.psm1'
  )
}
