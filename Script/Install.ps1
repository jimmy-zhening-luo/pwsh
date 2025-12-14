$Private:CMDLET_ROOT = "$PSScriptRoot\..\Cmdlet"

[hashtable]$Private:Compiled = @{
  Path = "$CMDLET_ROOT\CompleterBase\bin\Release\netstandard2.0\CompleterBase.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = $CMDLET_ROOT
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
