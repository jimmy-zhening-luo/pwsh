New-Alias guid Copy-Guid
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

  [void]($Guid | Set-Clipboard)
}
