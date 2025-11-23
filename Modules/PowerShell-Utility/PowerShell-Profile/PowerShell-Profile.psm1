New-Alias op Edit-Profile
function Edit-Profile {
  [OutputType([void])]
  param()

  Edit-Code -Path 'pwsh' -ProfileName PowerShell @args
}

New-Alias up Sync-Profile
function Sync-Profile {
  Get-Repository -Path '~\code\pwsh' && Sync-Linter
}
