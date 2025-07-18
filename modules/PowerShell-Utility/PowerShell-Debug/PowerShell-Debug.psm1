New-Alias mc Measure-Profile
function Measure-Profile {
  (
    Measure-Command {
      pwsh -Command 1
    }
  ).TotalMilliseconds
}

New-Alias mn Measure-NoProfile
function Measure-NoProfile {
  (
    Measure-Command {
      pwsh -NoProfile -Command 1
    }
  ).TotalMilliseconds
}
