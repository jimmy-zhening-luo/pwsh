New-Alias mc Measure-Command

New-Alias mcp Measure-PowerShellProfile
function Measure-PowerShellProfile {
  (
    Measure-Command {
      pwsh -Command 1
    }
  ).TotalMilliseconds
}

New-Alias mcnp Measure-PowerShellNoProfile
New-Alias mcn Measure-PowerShellNoProfile
function Measure-PowerShellNoProfile {
  (
    Measure-Command {
      pwsh -NoProfile -Command 1
    }
  ).TotalMilliseconds
}
