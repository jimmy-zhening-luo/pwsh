New-Alias cl Clear-Line
function Clear-Line {
  if ($args) {
    Clear-Content @args
  }
  else {
    Clear-Host
  }
}
