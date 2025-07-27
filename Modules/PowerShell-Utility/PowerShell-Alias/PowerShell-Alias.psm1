New-Alias galc Get-AliasCommand
<#
.FORWARDHELPTARGETNAME Get-Alias
#>
function Get-AliasCommand {
  param([System.String]$Definition = "*")

  Get-Alias -Definition ($Definition.Contains('*') ? $Definition : ($Definition.Length -lt 3 ? "$Definition*" : "*$Definition*")) |
    Select-Object DisplayName, Options, Source
}
