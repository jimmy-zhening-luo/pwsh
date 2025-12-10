using namespace System.IO

New-Alias split Split-Path
New-Alias hash Get-FileHash

New-Alias sz Get-Size
New-Alias size Get-Size
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
    [hashtable]$Private:UNITS = @{
      B  = 'B'
      KB = 'KB'
      MB = 'MB'
      GB = 'GB'
      K  = 'KB'
      M  = 'MB'
      G  = 'GB'
    }
    [hashtable]$Private:FACTORS = @{
      B  = 1
      KB = 1KB
      MB = 1MB
      GB = 1GB
    }
    [string]$Private:DEFAULT_PATH = $PWD.Path
    [string]$Private:DEFAULT_UNIT = 'KB'

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

    [hashtable]$Private:Target = @{
      Path = $Path
    }
    [string]$Private:UnitCanonical = $UNITS[$Unit]
    [FileSystemInfo]$Private:Item = Get-Item @Target
    [double]$Private:Quantity = [Math]::Round(
      (
        $Item.PSIsContainer ? (
          Get-ChildItem @Target -Recurse -File |
            Measure-Object -Property Length -Sum
        ).Sum : $Item.Length
      ) / [Int32]$FACTORS[$UnitCanonical],
      3
    )

    if ($QuantityOnly) {
      return $Quantity
    }
    else {
      return "$Quantity $UnitCanonical"
    }
  }
}
