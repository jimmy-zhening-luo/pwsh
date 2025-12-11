New-Alias sand Test-Sandbox
New-Alias sandbox Test-Sandbox
function Test-Sandbox {

  [OutputType([string[]])]

  param(

    [string]$Path,

    [switch]$Flag

  )

  'PSBoundParameters: ' + ($PSBoundParameters | ConvertTo-Json -EnumsAsStrings)
  "Path: $Path"
  "Args: $args"

  $args.gettype()
}
