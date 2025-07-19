New-Alias -Option ReadOnly -Name cl -Value Clear-Container
function Clear-Container {
  if ($args) {
    Clear-Content @args
  }
  else {
    Clear-Host
  }
}
