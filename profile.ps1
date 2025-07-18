New-Alias p Edit-Profile
function Open-ProfileRepository {
  Edit-File $PROFILE_REPO
}

New-Alias pi Initialize-Profile
function Initialize-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}

New-Alias pp Sync-ProfileRepository
function Sync-ProfileRepository {
  Get-Repository -Path $PROFILE_REPO
}
