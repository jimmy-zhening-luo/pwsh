@{
  ModuleVersion     = "1.0.0.0"
  GUID              = "7ef2a734-a882-4b6e-b3d9-2c43b1bdd3ed"
  NestedModules     = @(
    ".\Set-Folder"
    ".\Get-FolderChildItem"
    ".\Get-Property"
    ".\New-File"
    ".\New-Junction"
    ".\Compress-File"
    ".\Invoke-Folder"
    ".\Edit-File"
    ".\Clear-Line"
    ".\Remove-Folder"
  )
  FunctionsToExport = @(
    "Set-FolderCode"
    "Get-HomeItem"
    "Get-SiblingItem"
    "Get-Parent"
    "Get-FileSize"
    "New-Junction"
    "Compress-File"
    "Invoke-Folder"
    "Invoke-Parent"
    "Invoke-Home"
    "Invoke-Drive"
    "Edit-File"
    "Clear-Line"
    "Remove-Folder"
  )
  AliasesToExport   = @(
    "c"
    "c."
    "c.."
    "cd."
    "c~"
    "c\"
    "c/"
    "cc"
    "d\"
    "d/"
    "l"
    "l."
    "l.."
    "ls."
    "ls.."
    "l~"
    "ls~"
    "split"
    "parent"
    "hash"
    "size"
    "touch"
    "mk"
    "mj"
    "zip"
    "explore"
    "e"
    "e."
    "e.."
    "e~"
    "e\"
    "e/"
    "i"
    "cl"
  )
}
