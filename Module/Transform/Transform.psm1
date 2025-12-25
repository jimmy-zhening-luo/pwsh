function Test-Function {
  [CmdletBinding()]
  [OutputType([string])]
  param(

    [Parameter(
      Position = 0
    )]
    [string]$Name,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments
    )]
    [string[]]$ArgumentList,

    [Parameter(DontShow)][switch]$z
  )

  return ConvertTo-Json $PSBoundParameters -EnumsAsStrings -Depth 6
}

<#
.SYNOPSIS
Generate a new GUID and copy it to the clipboard.

.DESCRIPTION
This function generates a new GUID (globally unique identifier) and copies it to the clipboard.

.COMPONENT
Model

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
    [switch]$Silent,

    [Parameter(DontShow)][switch]$z
  )

  $Guid = (New-Guid).Guid

  if ($Uppercase) {
    $Guid = $Guid.ToUpperInvariant()
  }

  Set-Clipboard -Value $Guid

  if (-not $Silent) {
    return $Guid
  }
}

New-Alias test Test-Cmdlet
New-Alias hex ConvertTo-Hex

New-Alias fest Test-Function
New-Alias guid Copy-Guid
