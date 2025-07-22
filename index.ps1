if (Test-Path -PathType Container $HOME\code) {
  $code = "$HOME\code"

  if (Test-Path -PathType Container "$PSScriptRoot\Modules") {
    $Env:PSModulePath += ";$PSScriptRoot\Modules"

    try {
      $PSDefaultParameterValues = Import-PowerShellDataFile -ErrorAction Stop "$PSScriptRoot\defaults.psd1"

      . $PSScriptRoot\profile.ps1
      . $PSScriptRoot\data\index.ps1
      . $PSScriptRoot\alias\index.ps1
    }
    catch {
      throw "Failed to initialize PowerShell profile: $($_.Exception.Message)"
    }
  }
  else {
    Write-Warning "Skipping custom PowerShell profile: failed to find required Modules at $PSScriptRoot\Modules."
  }
}
else {
  Write-Warning "Skipping custom PowerShell profile: failed to find required PowerShell code repository at $HOME\code."
}
