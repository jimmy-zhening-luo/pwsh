New-Alias hex ConvertTo-Hex
function ConvertTo-Hex {
  [OutputType([string[]])]
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

  if ($Hex) {
    $Hex
  }
}

New-Alias hexl ConvertTo-HexLower
function ConvertTo-HexLower {
  [OutputType([string[]])]
  param(
    [Alias('Number')]
    [int[]]$Decimal
  )

  ConvertTo-Hex -Lowercase @PSBoundParameters
}
