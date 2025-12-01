New-Alias op PSTool\Invoke-PSProfile
function Invoke-PSProfile {
  Shell\Invoke-WorkspaceCode -Path 'pwsh' -ProfileName PowerShell @args
}

New-Alias up PSTool\Update-PSProfile
function Update-PSProfile {
  Git\Get-Repository -Path '~\code\pwsh' && Update-PSLinter
}
