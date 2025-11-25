New-Alias cl Clear-Line
function Clear-Line {
  [OutputType([void])]
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
