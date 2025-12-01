New-Alias sand Sandbox\Test-Sandbox
New-Alias sandbox Sandbox\Test-Sandbox
function Test-Sandbox {
  param (
    [string]$Path
  )

  "Path: $Path"
  "Args: $args"
}
