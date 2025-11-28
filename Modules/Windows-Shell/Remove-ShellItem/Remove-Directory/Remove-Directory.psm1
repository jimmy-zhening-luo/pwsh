function Remove-Directory {
  [CmdletBinding(SupportsShouldProcess)]
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions('.')]
    [string]$Path
  )

  if (
    $PSCmdlet.ShouldProcess(
      $Path,
      'Remove-Item -Recurse -Force'
    )
  ) {
    Remove-Item @PSBoundParameters -Recurse -Force
  }
}
