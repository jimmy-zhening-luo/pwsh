New-Alias hex Quick\ConvertTo-Hex
function ConvertTo-Hex {
  param(
    [Alias('Number')]
    [int[]]$Decimal,
    [Alias('Case', 'lc')]
    [switch]$Lowercase
  )

  $Hex = $Decimal | % { '{0:X}' -f $_ }

  if ($Lowercase) {
    $Hex | % { $_.ToLower() }
  }

  if ($Hex) { $Hex }
}

New-Alias hexl Quick\ConvertTo-HexLower
function ConvertTo-HexLower {
  param(
    [Alias('Number')]
    [int[]]$Decimal
  )

  ConvertTo-Hex -Lowercase @PSBoundParameters
}
