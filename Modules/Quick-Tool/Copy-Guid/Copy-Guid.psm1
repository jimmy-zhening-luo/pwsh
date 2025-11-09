New-Alias guid Copy-Guid
<#
.FORWARDHELPTARGETNAME New-Guid
.FORWARDHELPCATEGORY Cmdlet
#>
function Copy-Guid {
  [OutputType([void], [string])]
  param(
    [switch]$Upper,
    [switch]$Silent
  )
  $Guid = New-Guid |
    Select-Object -ExpandProperty Guid

  if ($Upper) {
    $Guid = $Guid.ToUpperInvariant()
  }

  if (-not $Silent) {
    $Guid
  }

  $Guid |
    Set-Clipboard
}
