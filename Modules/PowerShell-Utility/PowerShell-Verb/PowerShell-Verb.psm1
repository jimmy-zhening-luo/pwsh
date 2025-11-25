<#
.SYNOPSIS
Gets a list of approved PowerShell verbs.

.DESCRIPTION
The 'Get-VerbPowerShell' function gets verbs that are approved for use in PowerShell commands.

It invokes 'Get-Verb', sorts the returned verbs alphabetically, and returns only the 'Verb' field as a 'String' array.

It supports both parameters of 'Get-Verb', '-Verb' and '-Group', but it treats '-Verb' as a wildcard search rather than an exact match.

.PARAMETER Verb
Specifies the verb to search for. The default value is '*'. If the value contains a wildcard, it is passed to 'Get-Verb' as-is. If the value is shorter than 3 characters, a wildcard is appended ('Verb*'). If the value is 3 characters or longer, wildcards are prepended and appended ('*Verb*').

.LINK
http://learn.microsoft.com/powershell/module/microsoft.powershell.utility/get-verb

.LINK
Get-Verb
#>
function Get-VerbPowerShell {
  param([string]$Verb = '*')

  $Verbs = @{
    Verb = $Verb.Contains('*') ? $Verb : $Verb.Length -lt 3 ? "$Verb*" : "*$Verb*"
  }

  Get-Verb @Verbs @args |
    Sort-Object -Property Verb |
    Select-Object -ExpandProperty Verb
}
