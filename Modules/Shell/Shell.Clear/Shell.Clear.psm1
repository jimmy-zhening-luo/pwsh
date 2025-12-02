New-Alias cl Shell\Clear-Line
function Clear-Line {
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($Path -or $args) {
    Clear-Content @PSBoundParameters @args
  }
  else {
    Clear-Host
  }
}

function Remove-Directory {
  [CmdletBinding(SupportsShouldProcess)]
  [OutputType([void])]
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
