New-Alias -Name guid -Value Copy-Guid
<#
.FORWARDHELPTARGETNAME New-Guid
#>
function Copy-Guid {
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
