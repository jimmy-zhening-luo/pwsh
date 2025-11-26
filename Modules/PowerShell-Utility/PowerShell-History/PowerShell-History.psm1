New-Alias oc Edit-History
function Edit-History {
  $File = @{
    Path        = Get-PSReadLineOption |
      Select-Object -ExpandProperty HistorySavePath
    ProfileName = 'PowerShell'
    Window      = $true
  }

  Edit-ShellItem @File
}
