New-Alias ^ Select-Object
New-Alias pick Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias n Measure-Object
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias format Format-Table

New-Alias oc PSTool\Invoke-PSHistory
function Invoke-PSHistory {
  $History = @{
    Path        = (Get-PSReadLineOption).HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $True
  }
  Shell\Invoke-Workspace @History
}

New-Alias op PSTool\Invoke-PSProfile
function Invoke-PSProfile {
  $ProfileRepository = @{
    Path        = 'pwsh'
    ProfileName = 'PowerShell'
  }
  Shell\Invoke-WorkspaceCode @ProfileRepository @args
}

New-Alias up PSTool\Update-PSProfile
function Update-PSProfile {
  $ProfileRepository = @{
    Path = Join-Path $HOME code\pwsh -Resolve
  }
  Git\Get-Repository @ProfileRepository
  Update-PSLinter
}

function Update-PSLinter {
  $Linter = @{
    Path     = "$HOME\code\pwsh\PSScriptAnalyzerSettings.psd1"
    PathType = 'Leaf'
  }
  if (Test-Path @Linter) {
    $Copy = @{
      Path        = $Linter.Path
      Destination = $HOME
    } 
    Copy-Item @Copy
  }
}

New-Alias mc PSTool\Measure-PSProfile
function Measure-PSProfile {
  $Test = @{
    Command = '1'
  }
  $StartupLoadProfile = (
    Measure-Command { pwsh @Test }
  ).TotalMilliseconds
  $NormalStartup = (
    Measure-Command { pwsh -NoProfile @Test }
  ).TotalMilliseconds
  $Performance = [Math]::Max(
    [Math]::Round($StartupLoadProfile - $NormalStartup),
    0
  )
  $Print_NormalStartup = [Math]::Round($NormalStartup)
  $Print_Performance = $Performance -lt 1 -and $Performance -gt -1 ? 0 : $Performance

  "$Print_Performance ms`n(Base: $Print_NormalStartup ms)"
}
