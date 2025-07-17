New-Alias clip Set-Clipboard # default: clip.exe

New-Alias guid Copy-NewGuid
function Copy-NewGuid {
  param (
    [switch]$UpperCase,
    [switch]$NoOutput
  )
  $Guid = (New-Guid).Guid

  if ($UpperCase) {
    $Guid = $Guid.ToUpperInvariant()
  }

  if (-not $NoOutput) {
    Write-Output $Guid
  }

  $Guid | Set-Clipboard
}
