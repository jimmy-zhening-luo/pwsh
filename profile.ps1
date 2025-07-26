New-Alias -Option ReadOnly op Edit-Profile
function Edit-Profile {
  Edit-File $PSScriptRoot
}

New-Alias -Option ReadOnly up Sync-Profile
function Sync-Profile {
  Get-Repository -ErrorAction Stop $PSScriptRoot
  Sync-Linter
}

function Sync-Linter {
  $linter = "PSScriptAnalyzerSettings.psd1"

  Copy-Item $PSScriptRoot\$linter $HOME\$linter
}
