function Sync-Linter {
  [OutputType([void])]
  param()

  Copy-Item "~\code\pwsh\PSScriptAnalyzerSettings.psd1" "~\PSScriptAnalyzerSettings.psd1"
}
