New-Alias mc Measure-Profile
function Measure-Profile {
  $NoProfileLoadTime = (Measure-Command { pwsh -NoProfile -Command 1 }).TotalMilliseconds

  "$([math]::Round((Measure-Command { pwsh -Command 1 }).TotalMilliseconds - $NoProfileLoadTime)) ms  (Normal Startup: $([math]::Round($NoProfileLoadTime)) ms)"
}
