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

<#
.SYNOPSIS
Get the size of a file or directory.

.DESCRIPTION
Calculates and returns the size(s) of the specified filesystem object(s). For a file, returns the file size. For a directory, calculates and returns the total sum of sizes of all files recursively contained by the directory.

.COMPONENT
Shell.Get
#>
function Get-Size {
  [CmdletBinding(
    DefaultParameterSetName = 'String'
  )]
  [OutputType([string])]
  [OutputType(
    [double],
    ParameterSetName = 'Number'
  )]
  [Alias('sz', 'size')]
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
    [RelativePathCompletions()]
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
    [EnumCompletions(
      [DiskSizeUnit]
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
    switch ($Path) {
      {
        -not (Test-Path $PSItem)
      } {
        throw "Path '$PSItem' does not exist."
      }
      default {
        $Size = (
          Test-Path $PSItem -PathType Container
        ) ? (
          Get-ChildItem -Path $PSItem -Recurse -Force -File |
            Measure-Object -Property Length -Sum |
            Select-Object -ExpandProperty Sum
        ) : (
          Get-Item -Path $PSItem
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
  }

  end {
    if (-not $Path) {
      $Size = Get-ChildItem -Path $PWD.Path -Recurse -File |
        Measure-Object -Property Length -Sum |
        Select-Object -ExpandProperty Sum

      [double]$ScaledSize = $Size / $Factor

      if ($Number) {
        return $ScaledSize
      }
      else {
        return [System.Math]::Round(
          $ScaledSize,
          3
        ).ToString() + " $CanonicalUnit"
      }
    }
  }
}

<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-Directory {
  [CmdletBinding(
    DefaultParameterSetName = 'Items',
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo], [System.IO.FileInfo])]
  [Alias('l')]
  param(

    [Parameter(
      ParameterSetName = 'Items',
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      '',
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [Parameter(
      ParameterSetName = 'LiteralItems',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [Alias('PSPath', 'LP')]
    [string[]]$LiteralPath,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('s', 'r')]
    [switch]$Recurse,

    [uint]$Depth,

    [Alias('f')]
    [switch]$Force,

    [switch]$Name,

    [Alias('ad')]
    [switch]$Directory,

    [Alias('af')]
    [switch]$File,

    [Alias('ah', 'h')]
    [switch]$Hidden,

    [Alias('as')]
    [switch]$System,

    [Alias('ar')]
    [switch]$ReadOnly,

    [switch]$FollowSymlink,

    [System.Management.Automation.FlagsExpression[System.IO.FileAttributes]]$Attributes
  )

  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Get-ChildItem', [System.Management.Automation.CommandTypes]::Cmdlet)
    $scriptCmd = { & $wrappedCmd @PSBoundParameters }
    $steppablePipeline = $scriptCmd.GetSteppablePipeline()
    $steppablePipeline.Begin($PSCmdlet)
  }

  process {
    $steppablePipeline.Process($PSItem)
  }

  end {
    $steppablePipeline.End()
  }
}

<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-DirectorySibling {
  [CmdletBinding(
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo], [System.IO.FileInfo])]
  [Alias('l.')]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return Split-Path $PWD.Path },
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('s', 'r')]
    [switch]$Recurse,

    [uint]$Depth,

    [Alias('f')]
    [switch]$Force,

    [switch]$Name,

    [Alias('ad')]
    [switch]$Directory,

    [Alias('af')]
    [switch]$File,

    [Alias('ah', 'h')]
    [switch]$Hidden,

    [Alias('as')]
    [switch]$System,

    [Alias('ar')]
    [switch]$ReadOnly,

    [switch]$FollowSymlink,

    [System.Management.Automation.FlagsExpression[System.IO.FileAttributes]]$Attributes
  )

  process {
    $PSBoundParameters.Path = Join-Path (Split-Path $PWD.Path) $Path
    Get-ChildItem @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-DirectoryRelative {
  [CmdletBinding(
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo], [System.IO.FileInfo])]
  [Alias('l..')]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path },
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('s', 'r')]
    [switch]$Recurse,

    [uint]$Depth,

    [Alias('f')]
    [switch]$Force,

    [switch]$Name,

    [Alias('ad')]
    [switch]$Directory,

    [Alias('af')]
    [switch]$File,

    [Alias('ah', 'h')]
    [switch]$Hidden,

    [Alias('as')]
    [switch]$System,

    [Alias('ar')]
    [switch]$ReadOnly,

    [switch]$FollowSymlink,

    [System.Management.Automation.FlagsExpression[System.IO.FileAttributes]]$Attributes
  )

  process {
    $PSBoundParameters.Path = Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
    Get-ChildItem @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-DirectoryHome {
  [CmdletBinding(
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo], [System.IO.FileInfo])]
  [Alias('lh')]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [AllowNull()]
    [SupportsWildcards()]
    [PathCompletions(
      '~',
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('s', 'r')]
    [switch]$Recurse,

    [uint]$Depth,

    [Alias('f')]
    [switch]$Force,

    [switch]$Name,

    [Alias('ad')]
    [switch]$Directory,

    [Alias('af')]
    [switch]$File,

    [Alias('ah', 'h')]
    [switch]$Hidden,

    [Alias('as')]
    [switch]$System,

    [Alias('ar')]
    [switch]$ReadOnly,

    [switch]$FollowSymlink,

    [System.Management.Automation.FlagsExpression[System.IO.FileAttributes]]$Attributes
  )

  process {
    $PSBoundParameters.Path = Join-Path $HOME $Path
    Get-ChildItem @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-DirectoryCode {
  [CmdletBinding(
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo], [System.IO.FileInfo])]
  [Alias('lc')]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [AllowNull()]
    [SupportsWildcards()]
    [PathCompletions(
      '~\code',
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('s', 'r')]
    [switch]$Recurse,

    [uint]$Depth,

    [Alias('f')]
    [switch]$Force,

    [switch]$Name,

    [Alias('ad')]
    [switch]$Directory,

    [Alias('af')]
    [switch]$File,

    [Alias('ah', 'h')]
    [switch]$Hidden,

    [Alias('as')]
    [switch]$System,

    [Alias('ar')]
    [switch]$ReadOnly,

    [switch]$FollowSymlink,

    [System.Management.Automation.FlagsExpression[System.IO.FileAttributes]]$Attributes
  )

  process {
    $PSBoundParameters.Path = Join-Path $REPO_ROOT $Path
    Get-ChildItem @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Get-ChildItem
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-DirectoryDrive {
  [CmdletBinding(
    SupportsTransactions
  )]
  [OutputType([System.IO.DirectoryInfo], [System.IO.FileInfo])]
  [Alias('l/')]
  param(

    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowEmptyCollection()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Drive.Root },
      [PathItemType]::Directory
    )]
    [string[]]$Path,

    [Parameter(
      Position = 1
    )]
    [SupportsWildcards()]
    [string]$Filter,

    [SupportsWildcards()]
    [string[]]$Include,

    [SupportsWildcards()]
    [string[]]$Exclude,

    [Alias('s', 'r')]
    [switch]$Recurse,

    [uint]$Depth,

    [Alias('f')]
    [switch]$Force,

    [switch]$Name,

    [Alias('ad')]
    [switch]$Directory,

    [Alias('af')]
    [switch]$File,

    [Alias('ah', 'h')]
    [switch]$Hidden,

    [Alias('as')]
    [switch]$System,

    [Alias('ar')]
    [switch]$ReadOnly,

    [switch]$FollowSymlink,

    [System.Management.Automation.FlagsExpression[System.IO.FileAttributes]]$Attributes
  )

  process {
    $PSBoundParameters.Path = Join-Path $PWD.Drive.Root $Path
    Get-ChildItem @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-File {

  [OutputType([string[]])]
  [Alias('p')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [RelativePathCompletions(
      '',
      [PathItemType]::File
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

  [string]$FullPath = $Location ? (
    Join-Path $Location $Path
  ) : $Path

  if (-not (Test-Path $FullPath)) {
    throw "Path '$Target' does not exist."
  }
  elseif (-not (Test-Path $FullPath -PathType Leaf)) {
    throw "Path '$Target' is not a leaf item."
  }

  return Get-Content -Path $FullPath @Argument @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileSibling {

  [OutputType([string])]
  [Alias('p.')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [RelativePathCompletions(
      { return Split-Path $PWD.Path },
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location (Split-Path $PWD.Path) @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileRelative {

  [OutputType([string])]
  [Alias('p..')]
  param(

    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path },
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location ($PWD.Path | Split-Path | Split-Path) @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileHome {

  [OutputType([string])]
  [Alias('ph')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions(
      '~',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $HOME @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileCode {

  [OutputType([string])]
  [Alias('pc')]
  param(

    [PathCompletions(
      '~\code',
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $REPO_ROOT @args
}

<#
.FORWARDHELPTARGETNAME Get-Content
.FORWARDHELPCATEGORY Cmdlet
#>
function Get-FileDrive {

  [OutputType([string])]
  [Alias('p/')]
  param(

    [Parameter(
      Mandatory,
      Position = 0
    )]
    [RelativePathCompletions(
      { return $PWD.Drive.Root },
      [PathItemType]::File
    )]
    [string]$Path
  )

  Get-File -Path $Path -Location $PWD.Drive.Root @args
}

New-Alias split Split-Path
New-Alias hash Get-FileHash
