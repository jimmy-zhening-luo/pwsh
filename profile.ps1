New-Alias op Open-Profile
function Open-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}

New-Alias opp Open-ProfileRepository
function Open-ProfileRepository {
  Edit-File $PROFILE_REPO
}

New-Alias ops Sync-ProfileRepository
function Sync-ProfileRepository {
  Get-Repository -Path $PROFILE_REPO
}
