New-Alias -Name edit -Value Edit-File -Option ReadOnly
New-Alias -Name i -Value Edit-File -Option ReadOnly
function Edit-File {
  param([string]$Path)

  if ($Path) {
    if (Test-Path $Path) {
      code $Path @args
    }
    else {
      if ($Path.StartsWith("-")) {
        code . $Path @args
      }
      else {
        throw "File '$Path' does not exist."
      }
    }
  }
  else {
    code .
  }
}
