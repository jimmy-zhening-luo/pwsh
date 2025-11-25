New-Alias galc Get-AliasCommand
function Get-AliasCommand {
  param(
    [Alias('Command')]
    [string]$Definition = '*'
  )

  $Parameters = @{
    Definition = $Definition.Contains('*') ? $Definition : $Definition.Length -lt 3 ? "$Definition*" : "*$Definition*"
  }

  Get-Alias @Parameters @args |
    Select-Object DisplayName, Options, Source
}
