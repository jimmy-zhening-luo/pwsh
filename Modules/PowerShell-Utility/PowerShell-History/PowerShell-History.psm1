New-Alias -Option ReadOnly -Name ch -Value Edit-History
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
