function Edit-Linter {
  Edit-File $DEV_DRIVE\PSScriptAnalyzerSettings.psd1 PowerShell -NewWindow @args
}

function Sync-Linter {
  Copy-Item $PROFILE_SRC\PSScriptAnalyzerSettings.psd1 $DEV_DRIVE\PSScriptAnalyzerSettings.psd1
}
