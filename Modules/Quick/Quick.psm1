New-Alias guid Quick\Copy-Guid
function Copy-Guid {
  [OutputType([string])]
  param(
    [Alias('Case')]
    [switch]$Uppercase,
    [switch]$Silent
  )

  $Guid = New-Guid | Select-Object -ExpandProperty Guid

  if ($Uppercase) {
    $Guid = $Guid.ToUpperInvariant()
  }

  [void]($Guid | Set-Clipboard)

  if (-not $Silent) {
    $Guid
  }
}

New-Alias hex Quick\ConvertTo-Hex
function ConvertTo-Hex {
  [OutputType([string])]
  param(
    [Alias('Number')]
    [int[]]$Decimal,
    [Alias('Case')]
    [switch]$Lowercase
  )

  $Hex = $Decimal | % { '{0:X}' -f $_ }

  if ($Hex) {
    if ($Lowercase) {
      $Hex | % { $_.ToLower() }
    }

    $Hex
  }
}
