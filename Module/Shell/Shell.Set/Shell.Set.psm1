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
    Set-PrivateDirectory @PSBoundParameters
  }
}

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
    Set-PrivateDirectory @PSBoundParameters
  }
}

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
    [SupportsWildcards()]
    [RelativePathCompletions(
      '\',
      [PathItemType]::Directory
    )]
    [string]$Path,

    [switch]$PassThru
  )

  process {
    $PSBoundParameters.Path = Join-Path $PWD.Drive.Root $Path
    Set-PrivateDirectory @PSBoundParameters
  }
}

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
    Set-PrivateDirectory @PSBoundParameters
  }
}
