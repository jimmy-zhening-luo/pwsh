@{
  RootModule = "Git-Client.psm1"
  ModuleVersion = "1.0"
  GUID = "d8a43de0-682d-42d5-8333-bfbc80d84ac4"
  NestedModules = @(
    "Invoke-Repository"
    "Verbs\Add-Repository"
    "Verbs\Clone-Repository"
    "Verbs\Commit-Repository"
    "Verbs\Pull-Repository"
    "Verbs\Push-Repository"
    "Verbs\Reset-Repository"
  )
  AliasesToExport = @(
    "gitc"
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
