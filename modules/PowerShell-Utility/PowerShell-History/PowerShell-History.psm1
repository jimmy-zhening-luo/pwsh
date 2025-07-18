New-Alias oc Open-ConsoleHistory
function Open-ConsoleHistory {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
