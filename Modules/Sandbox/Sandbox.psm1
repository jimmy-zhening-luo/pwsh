#Requires -Modules Microsoft.PowerShell.Utility

Microsoft.PowerShell.Utility\New-Alias sand Sandbox\Test-Sandbox
Microsoft.PowerShell.Utility\New-Alias sandbox Sandbox\Test-Sandbox
function Test-Sandbox {
  [OutputType([string[]])]
  param (
    [string]$Path,
    [switch]$Flag
  )

  'PSBoundParameters: ' + ($PSBoundParameters | Microsoft.PowerShell.Utility\ConvertTo-Json -EnumsAsStrings)
  "Path: $Path"
  "Args: $args"
}
