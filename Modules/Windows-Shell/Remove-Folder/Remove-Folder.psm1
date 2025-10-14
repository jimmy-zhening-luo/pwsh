function Remove-Folder {
  [CmdletBinding(SupportsShouldProcess)]
  param(
    [Parameter(
      Position = 0,
      Mandatory
    )]
    [System.String]$Path
  )

  if (
    $PSCmdlet.ShouldProcess(
      $Path,
      "Remove-Item -Recurse -Force"
    )
  ) {
    Remove-Item $Path -Recurse -Force
  }
}
