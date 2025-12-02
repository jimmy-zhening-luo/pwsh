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
  Shell\Invoke-WorkspaceCode -Path pwsh -ProfileName PowerShell @args
}

New-Alias up PSTool\Update-PSProfile
function Update-PSProfile {
  Git\Get-Repository -Path $HOME\code\pwsh && Update-PSLinter
}

function Update-PSLinter {
  $Copy = @{
    Path        = "$HOME\code\pwsh\PSScriptAnalyzerSettings.psd1"
    Destination = $HOME
  }

  Copy-Item @Copy
}

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
