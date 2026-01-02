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
    Write-Output "Switch: $Switch"
    Write-Output (
      'Switch is true: ' + (
        $Switch ? 'Yes' : 'No'
      )
    )
    Write-Output (
      $PSBoundParameters | ConvertTo-Json -Depth 6 -EnumsAsStrings
    )
  }
}
