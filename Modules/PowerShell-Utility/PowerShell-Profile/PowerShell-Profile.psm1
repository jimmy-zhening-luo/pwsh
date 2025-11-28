New-Alias op Invoke-PSProfile
function Invoke-PSProfile {
  Invoke-WorkspaceCode -Path 'pwsh' -ProfileName PowerShell @args
}

New-Alias up Update-PSProfile
function Update-PSProfile {
  Get-Repository -Path '~\code\pwsh' && Update-PSLinter
}
