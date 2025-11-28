New-Alias oc Invoke-PSHistory
function Invoke-PSHistory {
  $File = @{
    Path        = Get-PSReadLineOption |
      Select-Object -ExpandProperty HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $true
  }

  Invoke-Workspace @File
}
