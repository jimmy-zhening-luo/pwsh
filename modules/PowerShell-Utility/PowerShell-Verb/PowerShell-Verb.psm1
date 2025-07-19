<#
.SYNOPSIS
Gets a list of approved PowerShell verbs.

.DESCRIPTION
The `Get-VerbPowerShell` function gets verbs that are approved for use in PowerShell commands.

It invokes `Get-Verb`, sorts the returned verbs alphabetically, and returns only the `Verb` field as a `String` array.

.LINK
http://learn.microsoft.com/powershell/module/microsoft.powershell.utility/get-verb
.LINK
Get-Verb
#>
function Get-VerbPowerShell {
  [OutputType([string])]
  param (
    [string]$Verb = "*"
  )

  $VerbMatch = $Verb.Contains('*') ? $Verb : ($Verb.Length -lt 3 ? "$Verb*" : "*$Verb*")

  (Get-Verb -Verb $VerbMatch @args | Sort-Object -Property Verb | Select-Object Verb).Verb
}
