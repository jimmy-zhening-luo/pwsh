New-Alias split Split-Path
New-Alias parent Split-Path

New-Alias hash Get-FileHash

New-Alias size Get-FileSize
function Get-FileSize {
  param(
    [Parameter(ValueFromPipeline)]
    [System.String]$Path,
    [ArgumentCompletions(
      "B",
      "KB",
      "MB",
      "GB"
    )]
    [System.String]$Unit
  )
  process {
    $UNITS = @{
      B  = 1
      KB = 1KB
      MB = 1MB
      GB = 1GB
      K  = 1KB
      M  = 1MB
      G  = 1GB
    }
    $DEFAULT_UNIT = "KB"
    $DEFAULT_PATH = ".\"

    if ($Path) {
      if ($Unit) {
        if (-not ($UNITS.Contains($Unit))) {
          throw "Unknown unit '$Unit'. Allowed units: $($UNITS.Keys -join ', ')."
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
          throw "Unknown unit '$Unit'. Allowed units: $($UNITS.Keys -join ', ')."
        }
      }
      else {
        $Path = $DEFAULT_PATH
        $Unit = $DEFAULT_UNIT

      }
    }

    $Item = Get-Item $Path

    [math]::Round(
      (
        (
          $Item.PSIsContainer
        ) ? (
          (
            Get-ChildItem $Path -Recurse -File |
              Measure-Object -Property Length -Sum
          ).Sum
        ) : (
          $Item.Length
        )
      ) / $UNITS[$Unit],
      3
    )
  }
}
