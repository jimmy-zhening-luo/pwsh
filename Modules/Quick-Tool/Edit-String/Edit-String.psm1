New-Alias plural Format-Count
function Format-Count {
  param(
    [Parameter(Mandatory)]
    [string]$Noun,
    [Parameter(
      Mandatory,
      ValueFromRemainingArguments
    )]
    [int[]]$Count
  )

  $Nouns = $Noun.Contains("/") ? $Noun.Split("/", 2) : @($Noun, "$($Noun)s")
  $Singular = $Nouns[0]
  $Plural = $Nouns[1]

  return $Count |
    % { "$_ $((($_ -eq 1) -or ($_ -eq -1)) ? $Singular : $Plural)" }
}
