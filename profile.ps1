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

  if (Test-Path -PathType Leaf $PSScriptRoot\$linter) {
    if (Test-Path -PathType Container $HOME\$linter) {
      Write-Warning "Linter path $linter already exists and is a directory."
    }
    else {
      Copy-Item $PSScriptRoot\$linter $HOME\$linter
    }
  }
  else {
    Write-Warning "Linter configuration missing from PowerShell profile repository."
  }
}
