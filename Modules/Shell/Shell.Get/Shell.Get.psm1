New-Alias split Split-Path
New-Alias hash Get-FileHash

New-Alias sz Shell\Get-Size
New-Alias size Shell\Get-Size
function Get-Size {
  [OutputType([string])]
  [OutputType([double], ParameterSetName = 'Number')]
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
    [Parameter(
      ParameterSetName = 'Number'
    )]
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
    $DEFAULT_PATH = (Get-Location).Path
    $DEFAULT_UNIT = 'KB'

    if ($Path) {
      if ($Unit) {
        if (-not ($UNITS.Contains($Unit))) {
          throw "Unknown unit '$Unit'. Allowed units: $($FACTORS.Keys -join ', ')"
        }
      }
      else {
        if (Test-Path $Path) {
          $Unit = $DEFAULT_UNIT
        }
        else {
          if ($UNITS.Contains($Path)) {
            $Unit, $Path = $Path, $DEFAULT_PATH
          }
          else {
            throw "Path '$Path' does not exist"
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
          throw "Unknown unit '$Unit'. Allowed units: $($FACTORS.Keys -join ', ')"
        }
      }
      else {
        $Path, $Unit = $DEFAULT_PATH, $DEFAULT_UNIT
      }
    }

    $UnitCanonical = $UNITS[$Unit]
    $Item = Get-Item $Path
    $Quantity = [Math]::Round(
      (
        $Item.PSIsContainer ? (
          Get-ChildItem -Path $Path -Recurse -File |
            Measure-Object -Property Length -Sum
        ).Sum : $Item.Length
      ) / $FACTORS[$UnitCanonical],
      3
    )

    $QuantityOnly ? [double]$Quantity : "$Quantity $UnitCanonical"
  }
}
