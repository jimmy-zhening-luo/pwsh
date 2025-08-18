New-Alias op Edit-Profile
function Edit-Profile {
  Edit-File $PROFILE_SRC
}

New-Alias up Sync-Profile
function Sync-Profile {
  Get-Repository $PROFILE_SRC && Sync-Linter
}
