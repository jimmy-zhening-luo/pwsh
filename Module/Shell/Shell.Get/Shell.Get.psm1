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

$DISK_SIZE_UNIT_ALIAS = @{
  K = [DiskSizeUnit]::KB
  M = [DiskSizeUnit]::MB
  G = [DiskSizeUnit]::GB
  T = [DiskSizeUnit]::TB
  P = [DiskSizeUnit]::PB
}

$DISK_SIZE_FACTORS = @{
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
      { return $PWD.Path }
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

    [Parameter(DontShow)][switch]$z
  )

  begin {
    [DiskSizeUnit]$CanonicalUnit = $null -eq [DiskSizeUnit]::$Unit ? $DISK_SIZE_UNIT_ALIAS.ContainsKey($Unit) ? [DiskSizeUnit]::($DISK_SIZE_UNIT_ALIAS[$Unit]) : [DiskSizeUnit]::KB : [DiskSizeUnit]::$Unit
    $Factor = $DISK_SIZE_FACTORS[$CanonicalUnit]
  }

  process {
    foreach ($filepath in $Path) {
      if (-not (Test-Path $filepath)) {
        throw "Path '$filepath' does not exist."
      }

      $Size = (
        Test-Path $filepath -PathType Container
      ) ? (
        Get-ChildItem -Path $filepath -Recurse -Force -File |
          Measure-Object -Property Length -Sum |
          Select-Object -ExpandProperty Sum
      ) : (
        Get-Item -Path $filepath
      ).Length

      [double]$ScaledSize = $Size / $Factor

      Write-Output (
        $Number ? $ScaledSize : (
          [System.Math]::Round(
            $ScaledSize,
            3
          ).ToString() + " $CanonicalUnit"
        )
      )
    }
  }

  end {
    if (-not $Path) {
      $Size = Get-ChildItem -Path $PWD.Path -Recurse -File |
        Measure-Object -Property Length -Sum |
        Select-Object -ExpandProperty Sum

      [double]$ScaledSize = $Size / $Factor

      return $Number ? $ScaledSize : (
        [System.Math]::Round(
          $ScaledSize,
          3
        ).ToString() + " $CanonicalUnit"
      )
    }
  }
}

function Get-Directory {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path },
      [PathItemType]::Directory
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
      { return Split-Path $PWD.Path },
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path (Split-Path $PWD.Path) $Path) @args
}

function Get-DirectoryRelative {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path },
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path ($PWD.Path | Split-Path | Split-Path) $Path) @args
}

function Get-DirectoryHome {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [PathCompletions(
      '~',
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $HOME $Path) @args
}

function Get-DirectoryCode {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::Directory
    )]
    [string]$Path
  )

  Get-ChildItem -Path (Join-Path $REPO_ROOT $Path) @args
}

function Get-DirectoryDrive {

  [OutputType([System.IO.DirectoryInfo[]], [System.IO.FileInfo[]])]
  param(

    [RelativePathCompletions(
      { return $PWD.Drive.Root },
      [PathItemType]::Directory
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
      { return $PWD.Path }
    )]
    [string]$Path,

    [string]$Location
  )

  [string[]]$Argument = @()

  if (
    $Location -and -not (
      Test-Path $Location -PathType Container
    )
  ) {
    $Argument += $Location
    $Location = [string]::Empty
  }

  if ($Path) {
    [string]$FullPath = $Location ? (
      Join-Path $Location $Path
    ) : $Path

    if (-not (Test-Path $FullPath)) {
      throw "Path '$Target' does not exist."
    }

    if (Test-Path $FullPath -PathType Container) {
      return Get-ChildItem -Path $FullPath @Argument @args
    }
    else {
      return Get-Content -Path $FullPath @Argument @args
    }
  }
  else {
    return Get-ChildItem -Path (
      (
        $Location ? (
          Resolve-Path $Location
        ) : $PWD
      ).Path
    ) @Argument @args
  }
}

function Get-FileSibling {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return Split-Path $PWD.Path }
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location (Split-Path $PWD.Path) @args
}

function Get-FileRelative {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path }
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location ($PWD.Path | Split-Path | Split-Path) @args
}

function Get-FileHome {

  [OutputType([string[]])]
  param(

    [PathCompletions(
      '~'
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $HOME @args
}

function Get-FileCode {

  [OutputType([string[]])]
  param(

    [PathCompletions(
      '~\code'
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $REPO_ROOT @args
}

function Get-FileDrive {

  [OutputType([string[]])]
  param(

    [RelativePathCompletions(
      { return $PWD.Drive.Root }
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
