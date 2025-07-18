@{
  ModuleVersion     = "1.2"
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
    "Edit-History"
    "Measure-Profile"
    "Measure-NoProfile"
  )
  AliasesToExport   = @(
    "galc"
    "m"
    "upman"
    "verb"
    "hh"
    "mc"
    "mn"
  )
}
