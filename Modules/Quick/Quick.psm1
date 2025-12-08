#Requires -Modules Microsoft.PowerShell.Management, Microsoft.PowerShell.Utility

Microsoft.PowerShell.Utility\New-Alias guid Quick\Copy-Guid
function Copy-Guid {
  [OutputType([string])]
  param(
    [Alias('Case')]
    [switch]$Uppercase,
    [switch]$Silent
  )

  $Guid = (Microsoft.PowerShell.Utility\New-Guid).Guid

  if ($Uppercase) {
    $Guid = $Guid.ToUpperInvariant()
  }

  [void]($Guid | Microsoft.PowerShell.Management\Set-Clipboard)

  if (-not $Silent) {
    $Guid
  }
}

Microsoft.PowerShell.Utility\New-Alias hex Quick\ConvertTo-Hex
function ConvertTo-Hex {
  [OutputType([string])]
  param(
    [Alias('Number')]
    [int[]]$Decimal,
    [Alias('Case')]
    [switch]$Lowercase
  )

  $Hex = $Decimal | ForEach-Object { '{0:X}' -f $PSItem }

  if ($Hex) {
    $Lowercase ? $Hex.ToLowerInvariant() : $Hex
  }
}
