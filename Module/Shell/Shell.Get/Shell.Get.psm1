using namespace System.IO
using namespace System.Collections.Generic
using namespace Completer

enum DiskSizeUnit {
  B
  KB
  MB
  GB
  TB
  PB
}

[hashtable]$DISK_SIZE_UNIT_ALIAS = @{
  K = [DiskSizeUnit]::KB
  M = [DiskSizeUnit]::MB
  G = [DiskSizeUnit]::GB
  T = [DiskSizeUnit]::TB
  P = [DiskSizeUnit]::PB
}

[hashtable]$DISK_SIZE_FACTORS = @{
  [DiskSizeUnit]::B  = 1
  [DiskSizeUnit]::KB = 1KB
  [DiskSizeUnit]::MB = 1MB
  [DiskSizeUnit]::GB = 1GB
  [DiskSizeUnit]::TB = 1TB
  [DiskSizeUnit]::PB = 1PB
}

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
    [StaticCompletions(
      'b,kb,mb,gb,tb,pb',
      $null, $null, $null
    )]
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
    [DiskSizeUnit]$CanonicalUnit = $null -eq [DiskSizeUnit]::$Unit ? $DISK_SIZE_UNIT_ALIAS.ContainsKey($Unit) ? [DiskSizeUnit]::($DISK_SIZE_UNIT_ALIAS[$Unit]) : [DiskSizeUnit]::KB : [DiskSizeUnit]::$Unit

    [UInt64]$Private:Factor = $DISK_SIZE_FACTORS[$CanonicalUnit]

    [List[UInt64]]$Sizes = [List[UInt64]]::new()
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

New-Alias split Split-Path
New-Alias hash Get-FileHash

New-Alias size Get-Size
New-Alias sz Get-Size
