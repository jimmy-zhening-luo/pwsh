New-Alias p Edit-Profile
function Edit-Profile {
  Edit-File $PROFILE_REPO
}

New-Alias pi Initialize-Profile
function Initialize-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}

New-Alias pp Sync-Profile
function Sync-Profile {
  Get-Repository -Path $PROFILE_REPO
}
