[hashtable]$Private:Compiled = @{
  Path = "$PSScriptRoot\..\Cmdlet\Good\bin\Release\net10.0\Good.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = "$PSScriptRoot\..\Module\Good"
    Force       = $True
  }
  [hashtable]$Private:ExistingInstall = @{
    Path = Join-Path $Install.Destination Good.dll
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
