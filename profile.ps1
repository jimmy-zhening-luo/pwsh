New-Alias -Option ReadOnly -Name op -Value Edit-Profile
function Edit-Profile {
  Edit-File -Path $PSScriptRoot
}

New-Alias -Option ReadOnly -Name os -Value Sync-Profile
function Sync-Profile {
  Get-Repository -Path $PSScriptRoot
}

New-Alias -Option ReadOnly -Name opi -Value Initialize-Profile
function Initialize-Profile {
  Edit-File -Path $PROFILE.CurrentUserAllHosts
}
