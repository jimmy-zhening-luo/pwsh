New-Alias sand Sandbox\Test-Sandbox
New-Alias sandbox Sandbox\Test-Sandbox
function Test-Sandbox {
  [OutputType([string[]])]
  param (
    [string]$Path,
    [switch]$Flag
  )

  'PSBoundParameters: ' + ($PSBoundParameters | ConvertTo-Json -EnumsAsStrings)
  "Path: $Path"
  "Args: $args"
}
