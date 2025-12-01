New-Alias guid Quick\Copy-Guid
function Copy-Guid {
  [OutputType([string])]
  param(
    [Alias('Case', 'uc')]
    [switch]$Uppercase,
    [switch]$Silent
  )

  $Guid = (New-Guid).Guid

  if ($Uppercase) {
    $Guid = $Guid.ToUpperInvariant()
  }

  if (-not $Silent) {
    $Guid
  }

  [void]($Guid | Set-Clipboard)
}

New-Alias hex Quick\ConvertTo-Hex
function ConvertTo-Hex {
  [OutputType([string])]
  param(
    [Alias('Number')]
    [int[]]$Decimal,
    [switch]$Lowercase
  )

  $Hex = $Decimal | % { '{0:X}' -f $_ }

  if ($Lowercase) {
    $Hex | % { $_.ToLower() }
  }

  if ($Hex) { $Hex }
}
