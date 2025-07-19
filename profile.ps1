New-Alias -Name op -Value Edit-Profile -Option ReadOnly
function Edit-Profile {
  Edit-File $PROFILE_REPO
}

New-Alias -Name opi -Value Initialize-Profile -Option ReadOnly
function Initialize-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}

New-Alias -Name os -Value Sync-Profile -Option ReadOnly
function Sync-Profile {
  Get-Repository -Path $PROFILE_REPO
}
