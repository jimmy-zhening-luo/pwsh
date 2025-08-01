@{
  RootModule        = "System-Command.psm1"
  ModuleVersion     = "1.0.0.0"
  GUID              = "9560b44f-3980-45d7-80f3-0c0f7d2ac2c4"
  NestedModules     = @(
    ".\Invoke-CommandPrompt"
    ".\Stop-Task"
    ".\Update-System"
  )
  FunctionsToExport = @(
    "Invoke-CommandPrompt"
    "Stop-Task"
    "Update-Windows"
    "Update-StoreApp"
  )
  AliasesToExport   = @(
    "restart"
    "sesv"
    "remsv"
    "cmd"
    "tkill"
    "tkillx"
    "wu"
    "su"
  )
}
