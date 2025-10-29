New-Alias p Get-File

function Get-File {
  param([System.String]$Path)

  if ($Path) {
    if (Test-Path $Path -PathType Leaf) {
      Get-Content $Path @args
    }
    elseif (Test-Path $Path -PathType Leaf) {
      Get-ChildItem $Path @args
    }
    else {
      throw "No file to print at path '$Path'"
    }
  }
  else {
    Get-ChildItem @args
  }
}
