@{
  ModuleVersion = "1.0"
  GUID = "d8a43de0-682d-42d5-8333-bfbc80d84ac4"
  NestedModules = @(
    ".\Invoke-Repository"
    # Verbs
    ".\Add-Repository"
    ".\Clone-Repository"
    ".\Commit-Repository"
    ".\Pull-Repository"
    ".\Push-Repository"
    ".\Reset-Repository"
  )
  AliasesToExport = @(
    "gitc"
    # Verbs
    "gita"
    "gitcl"
    "gitcp"
    "gitcr"
    "gitcrp"
    "gitm"
    "gitp"
    "gitpa"
  )
}
