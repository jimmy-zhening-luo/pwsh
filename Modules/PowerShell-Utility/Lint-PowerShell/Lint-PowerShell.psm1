function Update-PSLinter {
  $Copy = @{
    Path        = '~/code/pwsh/PSScriptAnalyzerSettings.psd1'
    Destination = '~'
  }

  Copy-Item @Copy
}
