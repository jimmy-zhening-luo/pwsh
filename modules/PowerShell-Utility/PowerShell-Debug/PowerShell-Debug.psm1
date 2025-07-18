New-Alias mc Measure-PowerShellProfile
function Measure-PowerShellProfile {
  (
    Measure-Command {
      pwsh -Command 1
    }
  ).TotalMilliseconds
}

New-Alias mcn Measure-PowerShellNoProfile
function Measure-PowerShellNoProfile {
  (
    Measure-Command {
      pwsh -NoProfile -Command 1
    }
  ).TotalMilliseconds
}

Export-ModuleMember Measure-PowerShellProfile, Measure-PowerShellNoProfile -Alias mc, mcn
