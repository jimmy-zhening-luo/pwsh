New-Alias op Edit-Profile
function Edit-Profile {
  [OutputType([void])]
  param()

  Edit-File $PROFILE_SRC PowerShell @args
}

New-Alias up Sync-Profile
function Sync-Profile {
  Get-Repository $PROFILE_SRC && Sync-Linter
}
