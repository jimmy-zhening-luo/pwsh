New-Alias sand Test-Sandbox
New-Alias sandbox Test-Sandbox
function Test-Sandbox {
  param (
    [string]$Path
  )

  "Path: $Path"
  "Args: $args"
}
