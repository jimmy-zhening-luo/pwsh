New-Alias oc PSTool\Invoke-PSHistory
function Invoke-PSHistory {
  $History = @{
    Path        = (Get-PSReadLineOption).HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $True
  }

  Shell\Invoke-Workspace @History
}
