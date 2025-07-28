New-Alias guid Copy-Guid
<#
.FORWARDHELPTARGETNAME New-Guid
.FORWARDHELPCATEGORY Cmdlet
#>
function Copy-Guid {
  param(
    [switch]$Upper,
    [switch]$Silent
  )
  $Guid = (New-Guid).Guid

  if ($Upper) {
    $Guid = $Guid.ToUpperInvariant()
  }

  if (-not $Silent) {
    $Guid
  }

  $Guid |
    Set-Clipboard
}
