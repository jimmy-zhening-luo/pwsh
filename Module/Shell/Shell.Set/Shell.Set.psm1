using namespace Completer.PathCompleter

function Set-Directory {
  [CmdletBinding(
    DefaultParameterSetName = 'Path'
  )]
  [OutputType(
    [System.Management.Automation.PathInfo],
    [System.Management.Automation.PathInfoStack]
  )]
  param(
    [Parameter(
      ParameterSetName = 'Path',
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Path },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [Parameter(
      ParameterSetName = 'LiteralPath',
      Mandatory,
      ValueFromPipelineByPropertyName
    )]
    [Alias('PSPath', 'LP')]
    [string]$LiteralPath,

    [Parameter(
      ParameterSetName = 'Stack',
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [string]$Stack,

    [switch]$PassThru
  )

  begin {
    $wrappedCmd = $ExecutionContext.InvokeCommand.GetCommand('Set-Location', [System.Management.Automation.CommandTypes]::Cmdlet)

    if (-not $Path -and -not $LiteralPath) {
      $PSBoundParameters.Path = Split-Path $PWD.Path
    }
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

function Set-DirectorySibling {
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return Split-Path $PWD.Path },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  $PSBoundParameters.Path = Join-Path (Split-Path $PWD.Path) $Path
  Set-Directory @PSBoundParameters
}

function Set-DirectoryRelative {
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  $PSBoundParameters.Path = Join-Path ($PWD.Path | Split-Path | Split-Path) $Path
  Set-Directory @PSBoundParameters
}

function Set-DirectoryHome {
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [PathCompletions(
      '~',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  $PSBoundParameters.Path = Join-Path $HOME $Path
  Set-Directory @PSBoundParameters
}

function Set-DirectoryCode {
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [PathCompletions(
      '~\code',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  $PSBoundParameters.Path = Join-Path $REPO_ROOT $Path
  Set-Directory @PSBoundParameters
}

function Set-Drive {
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Drive.Root },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  $PSBoundParameters.Path = Join-Path $PWD.Drive.Root $Path
  Set-Directory @PSBoundParameters
}

function Set-DriveD {
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowNull()]
    [AllowEmptyString()]
    [SupportsWildcards()]
    [PathCompletions(
      'D:',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  $PSBoundParameters.Path = Join-Path D: $Path
  Set-Directory @PSBoundParameters
}

New-Alias c Set-Directory
New-Alias c. Set-DirectorySibling
New-Alias c.. Set-DirectoryRelative
New-Alias ch Set-DirectoryHome
New-Alias cc Set-DirectoryCode
New-Alias c/ Set-Drive
New-Alias d/ Set-DriveD
