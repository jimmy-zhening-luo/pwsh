New-Alias p Get-File

function Get-File {
  if ($args) {
    Get-Content @args
  }
  else {
    Get-ChildItem
  }
}
