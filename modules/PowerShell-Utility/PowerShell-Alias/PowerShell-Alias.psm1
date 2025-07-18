New-Alias galc Get-AliasCommand
function Get-AliasCommand {
  param([string]$Definition)

  $Splat = $Definition ? @{
    Definition = (
      ($Definition.Length -lt 3) ? "" : "*"
    ) + $Definition + "*"
  } : @{}

  Get-Alias @Splat | Select-Object DisplayName
}
