New-Alias -Name mc -Value Measure-Profile -Option ReadOnly
function Measure-Profile {
  $TotalLoadTime = (
    Measure-Command {
      pwsh -Command 1
    }
  ).TotalMilliseconds

  $NoProfileLoadTime = (
    Measure-Command {
      pwsh -NoProfile -Command 1
    }
  ).TotalMilliseconds

  $ProfileOverhead = $TotalLoadTime - $NoProfileLoadTime

  Write-Output "$([math]::Round($ProfileOverhead)) ms  (Normal Startup: $([math]::Round($NoProfileLoadTime)) ms)"
}
