New-Alias -Name ch -Value Edit-History -Option ReadOnly
function Edit-History {
  Edit-File (Get-PSReadLineOption).HistorySavePath
}
