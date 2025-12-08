New-Alias sand Sandbox\Test-Sandbox
New-Alias sandbox Sandbox\Test-Sandbox
function Test-Sandbox {
  [OutputType([string[]])]
  param (
    [string]$Path,
    [Parameter(
      ParameterSetName = 'FlagSet'
    )]
    [switch]$Flag
  )

  "Path: $Path"
  "Args: $args"
  'ParameterSet: ' + $PSCmdlet.ParameterSetName
}
