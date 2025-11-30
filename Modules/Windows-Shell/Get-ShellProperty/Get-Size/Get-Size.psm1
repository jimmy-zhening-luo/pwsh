
New-Alias size Get-Size
function Get-Size {
  param(
    [Parameter(
      ValueFromPipeline,
      Position = 0
    )]
    [PathCompletions('.')]
    [string]$Path,
    [Parameter(Position = 1)]
    [GenericCompletions('B,KB,MB,GB')]
    [string]$Unit,
    [Alias('qo', 'Number')]
    [switch]$QuantityOnly
  )

  process {
    $UNITS = @{
      B  = 'B'
      KB = 'KB'
      MB = 'MB'
      GB = 'GB'
      K  = 'KB'
      M  = 'MB'
      G  = 'GB'
    }
    $FACTORS = @{
      B  = 1
      KB = 1KB
      MB = 1MB
      GB = 1GB
    }
    $DEFAULT_UNIT = 'KB'
    $DEFAULT_PATH = '.'

    if ($Path) {
      if ($Unit) {
        if (-not ($UNITS.Contains($Unit))) {
          throw "Unknown unit '$Unit'. Allowed units: $($FACTORS.Keys -join ', ')."
        }
      }
      else {
        if (Test-Path $Path) {
          $Unit = $DEFAULT_UNIT
        }
        else {
          if ($UNITS.Contains($Path)) {
            $Unit = $Path
            $Path = $DEFAULT_PATH
          }
          else {
            throw "'$Path' does not exist."
          }
        }
      }
    }
    else {
      if ($Unit) {
        if ($UNITS.Contains($Unit)) {
          $Path = $DEFAULT_PATH
        }
        else {
          throw "Unknown unit '$Unit'. Allowed units: $($FACTORS.Keys -join ', ')."
        }
      }
      else {
        $Path = $DEFAULT_PATH
        $Unit = $DEFAULT_UNIT

      }
    }

    $UnitCanonical = $UNITS[$Unit]
    $Item = Get-Item $Path
    $Quantity = [math]::Round(
      (
        $Item.PSIsContainer ? (
          Get-ChildItem -Path $Path -Recurse -File |
            Measure-Object -Property Length -Sum
        ).Sum : $Item.Length
      ) / $FACTORS[$UnitCanonical],
      3
    )

    if ($QuantityOnly) {
      $Quantity
    }
    else {
      "$Quantity $UnitCanonical"
    }
  }
}
