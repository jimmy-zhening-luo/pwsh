New-Alias oc Edit-History
function Edit-History {
  [OutputType([void])]
  param()

  [void](Edit-File (Get-PSReadLineOption).HistorySavePath PowerShell -NewWindow @args)
}
