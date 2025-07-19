New-Alias -Name cl -Value Clear-Container -Option ReadOnly
function Clear-Container {
  if ($args) {
    Clear-Content @args
  }
  else {
    Clear-Host
  }
}
