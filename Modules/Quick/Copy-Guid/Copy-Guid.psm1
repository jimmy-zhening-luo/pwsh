New-Alias guid Quick\Copy-Guid
function Copy-Guid {
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
