function Sync-Linter {
  [OutputType([void])]
  param()

  $Copy = @{
    Path        = '~/code/pwsh/PSScriptAnalyzerSettings.psd1'
    Destination = '~'
  }

  Copy-Item @Copy
}
