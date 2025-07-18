New-Alias verb Get-PowerShellVerb
function Get-PowerShellVerb {
  Get-Verb | Sort-Object -Property Verb | Select-Object Verb
}
