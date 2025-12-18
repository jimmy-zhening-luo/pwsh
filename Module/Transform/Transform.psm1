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
    [string[]]$ArgumentList

  )

  return $PSBoundParameters | ConvertTo-Json -EnumsAsStrings -Depth 6
}

New-Alias fest Test-Function

New-Alias test Test-Cmdlet
New-Alias hex ConvertTo-Hex
