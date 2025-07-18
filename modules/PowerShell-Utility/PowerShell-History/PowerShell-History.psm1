New-Alias hh Edit-History
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
