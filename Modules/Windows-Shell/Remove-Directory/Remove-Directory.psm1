function Remove-Directory {
  [OutputType([void])]
  [CmdletBinding(SupportsShouldProcess)]
  param(
    [Parameter(
      Mandatory,
      Position = 0
    )]
    [PathCompletions(".", "")]
    [string]$Path
  )

  if (
    $PSCmdlet.ShouldProcess(
      $Path,
      "Remove-Item -Recurse -Force"
    )
  ) {
    Remove-Item -Path $Path -Recurse -Force
  }
}
