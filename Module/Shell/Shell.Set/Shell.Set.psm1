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
