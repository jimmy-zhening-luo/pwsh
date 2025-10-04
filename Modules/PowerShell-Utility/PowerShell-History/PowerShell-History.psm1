New-Alias oc Edit-History
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath @args
}
