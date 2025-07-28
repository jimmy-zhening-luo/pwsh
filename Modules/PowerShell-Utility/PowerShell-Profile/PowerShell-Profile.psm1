New-Alias op Edit-Profile
function Edit-Profile {
  Edit-File $PROFILE_REPO
}

New-Alias up Sync-Profile
function Sync-Profile {
  Get-Repository $PROFILE_REPO && Sync-Linter
}
