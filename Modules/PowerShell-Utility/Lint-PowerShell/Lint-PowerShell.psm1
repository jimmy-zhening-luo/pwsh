function Sync-Linter {
  Copy-Item $PROFILE_REPO\PSScriptAnalyzerSettings.psd1 $HOME\PSScriptAnalyzerSettings.psd1
}
