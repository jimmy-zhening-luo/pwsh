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
    [Completions(
      'b,kb,mb,gb,tb,pb'
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

      [long]$Private:Size = (
        Test-Path -Path $Private:filepath -PathType Container
      ) ? (
        Get-ChildItem -Path $Private:filepath -Recurse -Force -File |
          Measure-Object -Property Length -Sum |
          Select-Object -ExpandProperty Sum
      ) : (
        Get-Item -Path $Private:filepath
      ).Length

      [double]$Private:ScaledSize = [long]$Private:Size / [long]$Private:Factor

      Write-Output -InputObject (
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

function Get-Directory {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  if ($Path) {
    Get-ChildItem -Path $Path @args
  }
  else {
    Get-ChildItem @args
  }
}

function Get-DirectorySibling {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

function Get-DirectoryRelative {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path ($PWD.Path | Split-Path | Split-Path) $Path) @args
}

function Get-DirectoryHome {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [LocationPathCompletions(
      '~',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $HOME $Path) @args
}

function Get-DirectoryCode {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [LocationPathCompletions(
      '~\code',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $REPO_ROOT $Path) @args
}

function Get-DirectoryDrive {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [LocationPathCompletions(
      '\',
      [PathItemType]::Directory,
      $null
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $PWD.Drive.Root $Path) @args
}

function Get-File {
  [OutputType(
    [string[]],
    [System.IO.DirectoryInfo[]],
    [System.IO.FileInfo[]]
  )]
  param(

    [RelativePathCompletions(
      { return [string]$PWD.Path },
      $null, $null
    )]
    [string]$Path,

    [string]$Location
  )

  [string[]]$Private:Argument = @()

  if (
    $Location -and -not (
      Test-Path -Path $Location -PathType Container
    )
  ) {
    $Private:Argument += $Location
    $Location = ''
  }

  if ($Path) {
    [string]$Private:FullPath = $Location ? (
      Join-Path $Location $Path
    ) : $Path

    if (-not (Test-Path -Path $Private:FullPath)) {
      throw "Path '$Private:Target' does not exist."
    }

    if (Test-Path -Path $Private:FullPath -PathType Container) {
      return Get-ChildItem -Path $Private:FullPath @Private:Argument @args
    }
    else {
      return Get-Content -Path $Private:FullPath @Private:Argument @args
    }
  }
  else {
    return Get-ChildItem -Path (
      (
        $Location ? (
          Resolve-Path -Path $Location
        ) : $PWD
      ).Path
    ) @Private:Argument @args
  }
}

function Get-FileSibling {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return [string](Split-Path $PWD.Path) },
      $null, $null
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location (Split-Path $PWD.Path) @args
}

function Get-FileRelative {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return [string]($PWD.Path | Split-Path | Split-Path) },
      $null, $null
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location ($PWD.Path | Split-Path | Split-Path) @args
}

function Get-FileHome {

  [OutputType([string[]])]
  param(

    [LocationPathCompletions(
      '~',
      $null, $null
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $HOME @args
}

function Get-FileCode {

  [OutputType([string[]])]
  param(

    [LocationPathCompletions(
      '~\code',
      $null, $null
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $REPO_ROOT @args
}

function Get-FileDrive {

  [OutputType([string[]])]
  param(

    [LocationPathCompletions(
      '\',
      $null, $null
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $PWD.Drive.Root @args
}

New-Alias split Split-Path
New-Alias hash Get-FileHash

New-Alias size Get-Size
New-Alias sz Get-Size

New-Alias l Get-Directory
New-Alias l. Get-DirectorySibling
New-Alias l.. Get-DirectoryRelative
New-Alias lh Get-DirectoryHome
New-Alias lc Get-DirectoryCode
New-Alias l/ Get-DirectoryDrive

New-Alias p Get-File
New-Alias p. Get-FileSibling
New-Alias p.. Get-FileRelative
New-Alias ph Get-FileHome
New-Alias pc Get-FileCode
New-Alias p/ Get-FileDrive
