New-Alias console Open-ConsoleHistory
New-Alias oc Open-ConsoleHistory
function Open-ConsoleHistory {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
