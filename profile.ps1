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
  Copy-Item $PSScriptRoot\PSScriptAnalyzerSettings.psd1 $HOME\PSScriptAnalyzerSettings.psd1
}
