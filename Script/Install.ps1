$Private:PROJECT_ROOT = "$PSScriptRoot\.."

[hashtable]$Private:Compiled = @{
  Path = "$PROJECT_ROOT\Cmdlet\CompleterBase\bin\Release\netstandard2.0\CompleterBase.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = "$PROJECT_ROOT\Module\CompleterBase\"
    Force       = $True
  }
  [hashtable]$Private:ExistingInstall = @{
    Path = Join-Path $Install.Destination CompleterBase.dll
  }
  if (
    -not (Test-Path @ExistingInstall) -or (
      Get-FileHash @ExistingInstall
    ).Hash -ne (
      Get-FileHash @Compiled
    ).Hash
  ) {
    Copy-Item @Compiled @Install
  }
}
