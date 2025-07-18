New-Alias wg winget
New-Alias wga Add-WinGetPackage
function Add-WinGetPackage {
  winget add @args
}
