New-Alias op Edit-Profile
function Edit-Profile {
  [OutputType([void])]
  param()

  Edit-Item -Path $PROFILE_SRC -ProfileName PowerShell @args
}

New-Alias up Sync-Profile
function Sync-Profile {
  Get-Repository -Path $PROFILE_SRC && Sync-Linter
}
