New-Alias hex ConvertTo-Hex
function ConvertTo-Hex {
  [OutputType([void], [string[]])]
  param(
    [int[]]$Decimal,
    [switch]$Lowercase
  )
  $Hex = $Decimal | % { '{0:X}' -f $_ }

  if ($Lowercase) {
    $Hex = $Hex | % { $_.ToLower() }
  }

  if ($Hex) { $Hex }
}

New-Alias hexl ConvertTo-HexLower
<#
.FORWARDHELPTARGETNAME ConvertTo-Hex
.FORWARDHELPCATEGORY Function
#>
function ConvertTo-HexLower {
  [OutputType([void], [string[]])]
  param([int[]]$Decimal)

  ConvertTo-Hex -Lowercase @PSBoundParameters
}
