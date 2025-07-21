New-Alias -Option ReadOnly op Edit-Profile
function Edit-Profile {
  try {
    Edit-File $PSScriptRoot
  }
  catch {
    throw "Failed to open PowerShell profile in Visual Studio Code: $($_.Exception.Message)"
  }
}

New-Alias -Option ReadOnly os Sync-Profile
function Sync-Profile {
  try {
    Get-Repository -ErrorAction Stop $PSScriptRoot
    try {
      Sync-Linter
    }
    catch {
      Write-Warning "Failed to synchronize linter configuration: $($_.Exception.Message)"
    }
  }
  catch {
    throw "Failed to synchronize PowerShell profile repository: $($_.Exception.Message)"
  }
}

function Sync-Linter {
  $linterName = "PSScriptAnalyzerSettings.psd1"

  if (Test-Path $PSScriptRoot\$linterName) {
    $linterFullName = "$HOME\$linterName"

    try {
      if (Test-Path $linterFullName) {
        Remove-Item -ErrorAction Stop -Force $linterFullName
      }

      Copy-Item "$PSScriptRoot\$linterName" $linterFullName
    }
    catch {
      throw "Failed to copy linter configuration from repository to ${HOME}: $($_.Exception.Message)"
    }
  }
  else {
    Write-Warning "No linter configuration '$linterName' found in PowerShell profile repository. If a linter configuration is already cached in $HOME, it will not be updated."
  }
}
