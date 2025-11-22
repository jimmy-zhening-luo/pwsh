New-Alias plural Format-Count
function Format-Count {
  [OutputType([void], [string[]])]
  param(
    [Parameter(Mandatory)]
    [string]$Noun,
    [Parameter(ValueFromRemainingArguments)]
    [string[]]$Count
  )

  if (-not $Noun) {
    throw 'Noun parameter is required.'
  }

  if ($Noun -as [int]) {
    if ($Count) {
      $Noun, $Count = $Count[-1], (, $Noun + $Count[0..($Count.Count - 2)])
    }
    else {
      $Noun, $Count = 'item', (, $Noun)
    }
  }

  if ($Noun -as [int]) {
    throw 'No noun provided'
  }

  $Nouns = $Noun.Contains('/') ? $Noun.Split('/', 2) : @(
    $Noun,
    "$($Noun)s"
  )

  $Count = $Count |
    ? { $_ -as [int] } |
    % { [int]$_ }

  if ($Count) {
    $Count |
      % { "$_ " + $Nouns[$_ -ne -1 -and $_ -ne 1] }
  }
}
