New-Alias c Set-Location
New-Alias c. cd..
New-Alias c.. cd..
New-Alias cd. cd..
New-Alias c~ cd~
New-Alias c\ cd\
New-Alias c/ cd\
New-Alias d\ D:
New-Alias d/ D:

New-Alias cc Set-FolderCode
function Set-FolderCode {
  Set-Location $code
}
