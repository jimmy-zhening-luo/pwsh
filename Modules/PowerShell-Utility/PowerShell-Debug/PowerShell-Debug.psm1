New-Alias mc Measure-PSProfile
function Measure-PSProfile {
  $Command = @{
    Command = '1'
  }
  $Unit = @{
    ExpandProperty = 'TotalMilliseconds'
  }

  $StartupTimeWithProfile = Measure-Command { pwsh @Command } |
    Select-Object @Unit
  $StartupTime = Measure-Command { pwsh -NoProfile @Command } |
    Select-Object @Unit
  $ProfileLoadTime = [math]::Round($StartupTimeWithProfile - $StartupTime)

  "$ProfileLoadTime ms`n(Base: $([math]::Round($StartupTime)) ms)"
}
