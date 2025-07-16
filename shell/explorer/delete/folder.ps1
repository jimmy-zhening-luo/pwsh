Set-Alias rd Remove-Folder # default: Remove-Item
function Remove-Folder {
  [CmdletBinding(SupportsShouldProcess)]
  param(
    [Parameter(
      Position = 0,
      Mandatory
    )]
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
