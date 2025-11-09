New-Alias cl Clear-Line
function Clear-Line {
  [OutputType([void])]
  param()

  if ($args) {
    Clear-Content @args
  }
  else {
    Clear-Host
  }
}
