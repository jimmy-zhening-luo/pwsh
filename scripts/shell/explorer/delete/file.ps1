Set-Alias rm Remove-File # default: Remove-Item
function Remove-File {
  [CmdletBinding(SupportsShouldProcess)]
  param(
    [Parameter(
      Position = 0,
      Mandatory
    )]
    [string]$Path,
    [Alias("r", "rf", "fr")]
    [switch]$Recurse
  )

  if (
    $PSCmdlet.ShouldProcess(
      $Path,
      "Remove-Item $($Recurse ? '-Recurse ' : '')-Force"
    )
  ) {
    if ($Recurse) {
      Remove-Item -Path $Path -Recurse -Force
    }
    else {
      Remove-Item -Path $Path -Force
    }
  }
}
