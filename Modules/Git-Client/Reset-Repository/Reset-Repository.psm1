New-Alias gitcr Undo-Repository
function Undo-Repository {
  param(
    [System.String]$Path
  )
  Invoke-Repository -Path $Path -Verb reset --hard
}

New-Alias gitcrp Restore-Repository
function Restore-Repository {
  param(
    [System.String]$Path
  )

  try {
    Undo-Repository -Path $Path
    Get-Repository -Path $Path
  }
  catch {
    throw ("Failed to reset and pull repository at '$Path' with message '$Message'. Caught error: " + $_.Exception.Message)
  }
}
