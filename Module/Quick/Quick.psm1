<#
.SYNOPSIS
Generate a new GUID and copy it to the clipboard.

.DESCRIPTION
This function generates a new GUID (globally unique identifier) and copies it to the clipboard.

.COMPONENT
PSTool

.LINK
https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/new-guid

.LINK
New-Guid
#>
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

<#
.SYNOPSIS
Convert integer(s) to hexadecimal string(s).

.DESCRIPTION
This function converts one or more integer values to their corresponding hexadecimal string representations.

.COMPONENT
PSTool
#>
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
    # Integer(s) to convert to hexadecimal
    [int[]]$Integer,

    [Alias('Case')]
    # Output lowercase hexadecimal string
    [switch]$Lowercase

  )

  [string]$Private:Hex = $Integer |
    ForEach-Object { '{0:X}' -f $PSItem }

  if ($Hex) {
    return $Lowercase ? $Hex.ToLowerInvariant() : $Hex
  }
}

New-Alias guid Copy-Guid
New-Alias hex ConvertTo-Hex
