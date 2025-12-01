New-Alias op Invoke-PSProfile
function Invoke-PSProfile {
  Shell\Invoke-WorkspaceCode -Path 'pwsh' -ProfileName PowerShell @args
}

New-Alias up Update-PSProfile
function Update-PSProfile {
  Git\Get-Repository -Path '~\code\pwsh' && Update-PSLinter
}
