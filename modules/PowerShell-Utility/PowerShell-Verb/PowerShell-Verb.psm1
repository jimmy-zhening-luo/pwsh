function Get-VerbPowerShell {
  (Get-Verb | Sort-Object -Property Verb | Select-Object Verb).Verb
}
