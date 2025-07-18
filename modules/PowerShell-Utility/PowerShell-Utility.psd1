@{
  ModuleVersion     = "1.0"
  GUID              = "c84491db-0b43-4dfc-80ea-890b16269a28"
  NestedModules     = @(
    ".\PowerShell-Alias"
    ".\PowerShell-Help"
    ".\PowerShell-Verb"
    ".\PowerShell-History"
    ".\PowerShell-Debug"
  )
  FunctionsToExport = @(
    "Get-AliasCommand"
    "Get-HelpOnline"
    "Get-PowerShellVerb"
    "Open-ConsoleHistory"
    "Measure-PowerShellProfile"
    "Measure-PowerShellNoProfile"
  )
  AliasesToExport   = @(
    "galc"
    "m"
    "upman"
    "verb"
    "oc"
    "mc"
    "mn"
  )
}
