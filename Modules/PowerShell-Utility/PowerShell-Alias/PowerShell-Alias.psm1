New-Alias galc Get-AliasCommand
<#
.FORWARDHELPTARGETNAME Get-Alias
#>
function Get-AliasCommand {
  param(
    [string]$Definition = "*"
  )

  $DefinitionMatch = $Definition.Contains('*') ? $Definition : ($Definition.Length -lt 3 ? "$Definition*" : "*$Definition*")

  Get-Alias -Definition $DefinitionMatch | Select-Object DisplayName, Options, Source
}
