function Update-PSLinter {
  $Copy = @{
    Path        = Join-Path $HOME 'code\pwsh\PSScriptAnalyzerSettings.psd1'
    Destination = $HOME
  }

  Copy-Item @Copy
}
