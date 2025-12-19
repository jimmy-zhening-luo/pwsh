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

    [Parameter(DontShow)][switch]$zNothing

  )

  [string]$Private:Guid = (New-Guid).Guid

  if ($Uppercase) {
    [string]$Private:Guid = $Private:Guid.ToUpperInvariant()
  }

  $Private:Guid | Set-Clipboard

  if (-not $Silent) {
    return $Private:Guid
  }
}

New-Alias ^ Select-Object
New-Alias t Select-Object
New-Alias to Select-Object
New-Alias k Get-Member
New-Alias key Get-Member
New-Alias count Measure-Object
New-Alias z Sort-Object
New-Alias format Format-Table

New-Alias guid Copy-Guid
