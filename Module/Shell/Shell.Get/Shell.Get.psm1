using namespace System.IO
using namespace System.Collections.Generic

New-Alias split Split-Path
New-Alias hash Get-FileHash

New-Alias size Get-Size
function Get-Size {

  [CmdletBinding(
    DefaultParameterSetName = 'String'
  )]

  [OutputType([string[]])]

  [OutputType([double[]], ParameterSetName = 'Number')]

  param(

    [Parameter(
      ParameterSetName = 'String',
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [Parameter(
      ParameterSetName = 'Number',
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [PathCompletions('.')]
    # The path of the file or directory to be measured.
    [string]$Path,

    [Parameter(
      ParameterSetName = 'String',
      Position = 1
    )]
    [Parameter(
      ParameterSetName = 'Number',
      Position = 1
    )]
    [GenericCompletions('B,KB,MB,GB,TB,PB')]
    # The unit in which to return the size.
    [string]$Unit,

    [Parameter(
      ParameterSetName = 'Number',
      Mandatory
    )]
    # Returns only the numeric scalar value in the specified unit.
    [switch]$Number

  )

  begin {
    [hashtable]$UNITS = @{
      B  = 'B'
      KB = 'KB'
      MB = 'MB'
      GB = 'GB'
      TB = 'TB'
      PB = 'PB'
      K  = 'KB'
      M  = 'MB'
      G  = 'GB'
      T  = 'TB'
      P  = 'PB'
    }
    [hashtable]$Private:FACTORS = @{
      B  = 1
      KB = 1KB
      MB = 1MB
      GB = 1GB
      TB = 1TB
      PB = 1PB
    }

    if ($Unit) {
      if (-not $UNITS.ContainsKey($Unit)) {
        throw "Unknown unit '$Unit'"
      }
    }
    else {
      $Unit = 'KB'
    }

    [string]$Private:CanonicalUnit = $UNITS[$Unit]
    [UInt64]$Private:Factor = $FACTORS[$CanonicalUnit]

    $Sizes = [List[UInt64]]::new()
  }

  process {
    if (-not $Path) {
      $Path = $PWD.Path
    }

    if (-not (Test-Path -Path $Path)) {
      throw "Path '$Path' does not exist."
    }

    [hashtable]$Private:Target = @{
      Path = $Path
    }
    [FileSystemInfo]$Private:Item = Get-Item @Target

    [UInt64]$Private:Size = $Item.PSIsContainer ? (
      Get-ChildItem @Target -Recurse -File |
        Measure-Object -Property Length -Sum
    ).Sum : $Item.Length

    $Sizes.Add($Size)
  }

  end {
    [double[]]$Private:ScaledSizes = $Sizes |
      ForEach-Object {
        $PSItem / $Factor
      }

    if ($Number) {
      return $ScaledSizes
    }
    else {
      [double[]]$RoundedSizes = $ScaledSizes |
        ForEach-Object {
          [System.Math]::Round($PSItem, 3)
        }
      [string[]]$Private:PrintedSizes = $RoundedSizes |
        ForEach-Object {
          "$PSItem $CanonicalUnit"
        }

      return $PrintedSizes
    }
  }
}
