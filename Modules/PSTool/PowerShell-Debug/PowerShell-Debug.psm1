New-Alias mc PSTool\Measure-PSProfile
function Measure-PSProfile {
  $Command = @{
    Command = '1'
  }
  $StartupTimeWithProfile = (
    Measure-Command { pwsh @Command }
  ).TotalMilliseconds
  $StartupTime = (
    Measure-Command { pwsh -NoProfile @Command }
  ).TotalMilliseconds
  $ProfileLoadTime = [math]::Round($StartupTimeWithProfile - $StartupTime)

  "$ProfileLoadTime ms`n(Base: $([math]::Round($StartupTime)) ms)"
}
