New-Alias op Edit-Profile
function Edit-Profile {
  Edit-ShellCode -Path 'pwsh' -ProfileName PowerShell @args
}

New-Alias up Sync-Profile
function Sync-Profile {
  Get-Repository -Path '~\code\pwsh' && Sync-Linter
}
