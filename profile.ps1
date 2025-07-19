New-Alias op Edit-Profile
function Edit-Profile {
  Edit-File $PROFILE_REPO
}

New-Alias oi Initialize-Profile
function Initialize-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}

New-Alias os Sync-Profile
function Sync-Profile {
  Get-Repository -Path $PROFILE_REPO
}
