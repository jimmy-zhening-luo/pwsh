New-Alias oc Edit-History
function Edit-History {
  [OutputType([void])]
  param()

  Edit-Item -Path (Get-PSReadLineOption).HistorySavePath -ProfileName PowerShell -Window @args
}
