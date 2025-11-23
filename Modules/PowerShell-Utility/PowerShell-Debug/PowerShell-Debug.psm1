New-Alias mc Measure-Profile
function Measure-Profile {
  [OutputType([string])]
  param()

  $Command = @{
    Command = '1'
  }
  $Unit = @{
    ExpandProperty = 'TotalMilliseconds'
  }

  $StartupTime = Measure-Command { pwsh -NoProfile @Command } |
    Select-Object @Unit
  $StartupTimeWithProfile = Measure-Command { pwsh @Command } |
    Select-Object @Unit
  $ProfileLoadTime = [math]::Round($StartupTimeWithProfile - $StartupTime)

  "$ProfileLoadTime ms`n(Base: $([math]::Round($StartupTime)) ms)"
}
