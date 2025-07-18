New-Alias gitcr Undo-Repository
function Undo-Repository {
  param(
    [string]$Path
  )
  Invoke-Repository -Path $Path -Verb reset --hard
}

New-Alias gitcrp Restore-Repository
function Restore-Repository {
  param(
    [string]$Path
  )

  try {
    Undo-Repository -Path $Path
    Get-Repository -Path $Path
  }
  catch {
    throw ("Failed to reset and pull repository at '$Path' with message '$Message'. Caught error: " + $_.Exception.Message)
  }
}

Export-ModuleMember Undo-Repository, Restore-Repository -Alias gitcr, gitcrp
