New-Alias guid Copy-Guid
function Copy-Guid {
  [CmdletBinding()]
  [OutputType([string])]
  param(
    [Alias('Case')]
    # Generate an uppercase GUID
    [switch]$Uppercase,
    # Only copy GUID to clipboard; do not output to the console
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
  [CmdletBinding()]
  [OutputType([string])]
  param(
    [Parameter(
      Mandatory,
      Position = 0,
      ValueFromRemainingArguments
    )]
    [AllowEmptyCollection()]
    [Alias('Number')]
    [int[]]$Integer,
    [Alias('Case')]
    [switch]$Lowercase
  )

  [string]$Private:Hex = $Integer |
    ForEach-Object { '{0:X}' -f $PSItem }

  if ($Hex) {
    return $Lowercase ? $Hex.ToLowerInvariant() : $Hex
  }
}
