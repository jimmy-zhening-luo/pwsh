New-Alias oc Edit-History
function Edit-History {
  $File = @{
    Path         = Get-PSReadLineOption |
      Select-Object -ExpandProperty HistorySavePath
    ProfileName  = 'PowerShell'
    CreateWindow = $true
  }

  Edit-ShellItem @File
}
