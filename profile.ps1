New-Alias -Name op -Value Edit-Profile
function Edit-Profile {
  Edit-File $PROFILE_REPO
}

New-Alias -Name opi -Value Initialize-Profile
function Initialize-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}

New-Alias -Name os -Value Sync-Profile
function Sync-Profile {
  Get-Repository -Path $PROFILE_REPO
}
