New-Alias galc Get-AliasCommand
<#
.FORWARDHELPTARGETNAME Get-Alias
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-AliasCommand {
  [OutputType([System.Management.Automation.AliasInfo[]])]
  param([string]$Definition = "*")

  Get-Alias -Definition ($Definition.Contains('*') ? $Definition : ($Definition.Length -lt 3 ? "$Definition*" : "*$Definition*")) |
    Select-Object DisplayName, Options, Source
}
