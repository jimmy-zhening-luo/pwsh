Set-Alias clear Clear-Container # default: Clear-Host
New-Alias cl Clear-Container
function Clear-Container {
  if ($args) {
    Clear-Content @args
  }
  else {
    Clear-Host
  }
}
