function Test-Function {
  [CmdletBinding()]
  [OutputType([string])]
  param(

    [Parameter(
      Position = 0
    )]
    [string]$Name,

    [Parameter(
      Position = 1,
      ValueFromRemainingArguments
    )]
    [string[]]$ArgumentList,

    [Parameter(DontShow)][switch]$z
  )

  return ConvertTo-Json $PSBoundParameters -EnumsAsStrings -Depth 6
}

New-Alias test Test-Cmdlet

New-Alias fest Test-Function
New-Alias guid Copy-Guid
New-Alias hex ConvertTo-Hex
