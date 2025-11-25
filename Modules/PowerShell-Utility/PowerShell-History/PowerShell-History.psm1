New-Alias oc Edit-History
function Edit-History {
  [OutputType([void])]
  param()

  $File = @{
    Path         = Get-PSReadLineOption |
      Select-Object -ExpandProperty HistorySavePath
    ProfileName  = 'PowerShell'
    CreateWindow = $true
  }

  Edit-Item @File
}
