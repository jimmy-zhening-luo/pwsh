@{
  ModuleVersion     = "1.0.0.0"
  GUID              = "d8a43de0-682d-42d5-8333-bfbc80d84ac4"
  NestedModules     = @(
    ".\Invoke-Repository"
    ".\Add-Repository"
    ".\Clone-Repository"
    ".\Commit-Repository"
    ".\Pull-Repository"
    ".\Push-Repository"
    ".\Reset-Repository"
  )
  FunctionsToExport = @(
    "Invoke-Repository"
    "Resolve-Repository"
    "Add-Repository"
    "Import-Repository"
    "Write-Repository"
    "Get-Repository"
    "Get-ChildRepository"
    "Push-Repository"
    "Undo-Repository"
    "Restore-Repository"
  )
  AliasesToExport   = @(
    "gitc"
    "gg"
    "gita"
    "gitcl"
    "gits"
    "ggs"
    "gitr"
    "gr"
    "gitrp"
    "grp"
    "gitm"
    "ggm"
    "gitp"
    "ggp"
    "gpa"
  )
}
