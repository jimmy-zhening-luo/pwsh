using namespace Completer
using namespace Completer.PathCompleter

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
    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    # The path of the file or directory to be measured.
    [string[]]$Path,

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
      $null, $null
    )]
    # The unit in which to return the size.
    [string]$Unit,

    [Parameter(
      ParameterSetName = 'Number',
      Mandatory
    )]
    # Returns only the numeric scalar value in the specified unit.
    [switch]$Number,

    [Parameter(DontShow)][switch]$zNothing
  )

  begin {
    [DiskSizeUnit]$Private:CanonicalUnit = $null -eq [DiskSizeUnit]::$Unit ? $DISK_SIZE_UNIT_ALIAS.ContainsKey($Unit) ? [DiskSizeUnit]::($DISK_SIZE_UNIT_ALIAS[$Unit]) : [DiskSizeUnit]::KB : [DiskSizeUnit]::$Unit
    [ulong]$Private:Factor = $DISK_SIZE_FACTORS[$Private:CanonicalUnit]
  }

  process {
    foreach ($Private:filepath in $Path) {
      if (-not (Test-Path -Path $Private:filepath)) {
        throw "Path '$Private:filepath' does not exist."
      }

      [hashtable]$Private:Target = @{
        Path = $Private:filepath
      }
      [System.IO.FileSystemInfo]$Private:Item = Get-Item @Private:Target

      [ulong]$Private:Size = $Private:Item.PSIsContainer ? (
        Get-ChildItem @Private:Target -Recurse -Force -File |
          Measure-Object -Property Length -Sum |
          Select-Object -ExpandProperty Sum
      ) : $Private:Item.Length

      [double]$Private:ScaledSize = $Private:Size / $Private:Factor

      Write-Output (
        $Number ? $Private:ScaledSize : (
          [System.Math]::Round(
            $Private:ScaledSize,
            3
          ).ToString() + ' ' + $Private:CanonicalUnit
        )
      )
    }
  }

  end {
    if (-not $Path) {
      [ulong]$Private:Size = Get-ChildItem -Path $PWD.Path -Recurse -File |
        Measure-Object -Property Length -Sum |
        Select-Object -ExpandProperty Sum

      [double]$Private:ScaledSize = $Private:Size / $Private:Factor

      return (
        $Number ? $Private:ScaledSize : (
          [System.Math]::Round(
            $Private:ScaledSize,
            3
          ).ToString() + ' ' + $Private:CanonicalUnit
        )
      )
    }
  }
}

New-Alias split Split-Path
New-Alias hash Get-FileHash

New-Alias size Get-Size
New-Alias sz Get-Size
