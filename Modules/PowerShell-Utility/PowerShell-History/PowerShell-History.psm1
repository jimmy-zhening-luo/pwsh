New-Alias ch Edit-History
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
