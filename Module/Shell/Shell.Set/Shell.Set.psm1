using namespace Completer.PathCompleter

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-Directory {
  [CmdletBinding(
    DefaultParameterSetName = 'Path'
  )]
  [OutputType(
    [System.Management.Automation.PathInfo],
    [System.Management.Automation.PathInfoStack]
  )]
  [Alias('c')]
  param(
    [Parameter(
      ParameterSetName = 'Path',
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      '',
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
    [AllowEmptyString()]
    [AllowNull()]
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

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-DirectorySibling {
  [CmdletBinding()]
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  [Alias('c.')]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return Split-Path $PWD.Path },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path (
      Split-Path $PWD.Path
    ) $Path
    Set-Directory @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-DirectoryRelative {
  [CmdletBinding()]
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  [Alias('c..')]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Path | Split-Path | Split-Path },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path (
      $PWD.Path | Split-Path | Split-Path
    ) $Path
    Set-Directory @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-DirectoryHome {
  [CmdletBinding()]
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  [Alias('ch')]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [PathCompletions(
      '~',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path $HOME $Path
    Set-Directory @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-DirectoryCode {
  [CmdletBinding()]
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  [Alias('cc')]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [PathCompletions(
      '~\code',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path $REPO_ROOT $Path
    Set-Directory @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-Drive {
  [CmdletBinding()]
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  [Alias('c/')]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [RelativePathCompletions(
      { return $PWD.Drive.Root },
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path $PWD.Drive.Root $Path
    Set-Directory @PSBoundParameters
  }
}

<#
.FORWARDHELPTARGETNAME Set-Location
.FORWARDHELPCATEGORY Cmdlet
#>
function Set-DriveD {
  [CmdletBinding()]
  [OutputType(
    [System.Management.Automation.PathInfo]
  )]
  [Alias('d/')]
  param(
    [Parameter(
      Position = 0,
      ValueFromPipeline,
      ValueFromPipelineByPropertyName
    )]
    [AllowEmptyString()]
    [AllowNull()]
    [SupportsWildcards()]
    [PathCompletions(
      'D:',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path D: $Path
    Set-Directory @PSBoundParameters
  }
}
