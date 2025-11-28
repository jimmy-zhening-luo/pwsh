New-Alias galc Get-CommandAlias
function Get-CommandAlias {
  param(
    [Alias('Command')]
    [string]$Definition = '*'
  )

  $Commands = @{
    Definition = $Definition.Contains('*') ? $Definition : $Definition.Length -lt 3 ? "$Definition*" : "*$Definition*"
  }

  Get-Alias @Commands @args |
    Select-Object DisplayName, Options, Source
}
