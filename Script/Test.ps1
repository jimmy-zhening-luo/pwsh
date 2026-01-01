
function Test-Function {
  [CmdletBinding()]
  [Alias('fest')]
  param (
    [string]$Name,
    [switch]$Switch
  )

  process {

  }

  end {
    Write-Output (
      $PSBoundParameters | ConvertTo-Json -Depth 6 -EnumsAsStrings
    )
  }
}
