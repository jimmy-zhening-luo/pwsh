New-Alias profile Open-Profile
New-Alias op Open-Profile
function Open-Profile {
  Edit-File $PROFILE.CurrentUserAllHosts
}
New-Alias opp Open-ProfileRepository
function Open-ProfileRepository {
  Edit-File $PROFILE_REPO
  Edit-File $PROFILE_REPO\windows\pwsh\index.ps1 --reuse-window
}
New-Alias ops Sync-ProfileRepository
function Sync-ProfileRepository {
  Get-Repository -Path $PROFILE_REPO
}
