New-Alias oc Edit-History
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath PowerShell -NewWindow @args
}
