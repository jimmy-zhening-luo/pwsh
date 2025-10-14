@{
  ModuleVersion     = "1.0.0.0"
  GUID              = "c84491db-0b43-4dfc-80ea-890b16269a28"
  NestedModules     = @(
    ".\PowerShell-Object"
    ".\PowerShell-Alias"
    ".\PowerShell-Help"
    ".\PowerShell-Verb"
    ".\PowerShell-Profile"
    ".\PowerShell-History"
    ".\PowerShell-Debug"
    ".\Lint-PowerShell"
  )
  FunctionsToExport = @(
    "Get-AliasCommand"
    "Get-HelpOnline"
    "Get-VerbPowerShell"
    "Edit-Profile"
    "Sync-Profile"
    "Edit-History"
    "Measure-Profile"
    "Edit-Linter"
    "Sync-Linter"
  )
  AliasesToExport   = @(
    "count"
    "n"
    "k"
    "pick"
    "to"
    "az"
    "format"
    "table"
    "wrap"
    "galc"
    "m"
    "upman"
    "op"
    "up"
    "oc"
    "mc"
  )
}
