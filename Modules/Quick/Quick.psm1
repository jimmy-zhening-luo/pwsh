New-Alias guid Copy-Guid
function Copy-Guid {
  [OutputType([string])]
  param(
    [Alias('Case')]
    [switch]$Uppercase,
    [switch]$Silent
  )

  [guid]$Private:Guid = New-Guid

  if ($Uppercase) {
    $Guid = [guid]$Guid.Guid.ToUpperInvariant()
  }

  $Guid | Set-Clipboard

  if (-not $Silent) {
    return [string]$Guid.Guid
  }
}

New-Alias hex ConvertTo-Hex
function ConvertTo-Hex {
  [OutputType([string])]
  param(
    [Parameter(
      Position = 0,
      Mandatory,
      ValueFromRemainingArguments
    )]
    [AllowEmptyCollection()]
    [Alias('Number')]
    [int[]]$Decimal,
    [Alias('Case')]
    [switch]$Lowercase
  )

  [string]$Private:Hex = $Decimal |
    ForEach-Object { '{0:X}' -f $PSItem }

  if ($Hex) {
    return $Lowercase ? $Hex.ToLowerInvariant() : $Hex
  }
}
