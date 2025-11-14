New-Alias cl Clear-Line
function Clear-Line {
  [OutputType([void])]
  param(
    [PathCompletions('.')]
    [string]$Path
  )

  if ($Path -or $args) {
    Clear-Content $Path @args
  }
  else {
    Clear-Host
  }
}
