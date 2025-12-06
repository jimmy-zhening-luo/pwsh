New-Alias cl Shell\Clear-Line
function Clear-Line {
  [OutputType([void])]
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($Path -or $args) {
    [void](Clear-Content @PSBoundParameters @args)
  }
  else {
    [void](Clear-Host)
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
    [void](Remove-Item @PSBoundParameters -Recurse -Force)
  }
}
