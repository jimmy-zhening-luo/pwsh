New-Alias -Option ReadOnly -Name edit -Value Edit-File
New-Alias -Option ReadOnly -Name i -Value Edit-File
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
