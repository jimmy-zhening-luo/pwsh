function Sync-Linter {
  Copy-Item $PROFILE_SRC\PSScriptAnalyzerSettings.psd1 $DEV_DRIVE\PSScriptAnalyzerSettings.psd1
}
