New-Alias oh Edit-History
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
