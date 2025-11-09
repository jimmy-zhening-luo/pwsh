function Sync-Linter {
  [OutputType([void])]
  param()

  Copy-Item $PROFILE_SRC\PSScriptAnalyzerSettings.psd1 $DEV_DRIVE\PSScriptAnalyzerSettings.psd1
}
