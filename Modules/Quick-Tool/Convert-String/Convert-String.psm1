New-Alias plural Format-Count
function Format-Count {
  [OutputType([void], [string[]])]
  param(
    [Parameter(Mandatory)]
    [string]$Noun,
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Count
  )

  if ($Noun -as [int]) {
    if ($Count -and $Count.Count -gt 0) {
      $Noun, $Count = $Count[-1], (, $Noun + $Count[0..($Count.Count - 2)])
    }
    else {
      $Count = , $Noun
      $Noun = "item"
    }
  }

  if ($Noun -as [int]) {
    throw "Noun parameter must be a string representing a noun."
  }

  $Nouns = $Noun.Contains("/") ? $Noun.Split("/", 2) : @(
    $Noun,
    "$($Noun)s"
  )

  if ($Count) {
    $Count = $Count |
      ? { $_ -as [int] } |
      % { [int]$_ }

    if ($Count) {
      $Count |
        % { "$_ " + $Nouns[($_ -ne -1) -and ($_ -ne 1)] }
    }
  }
}
