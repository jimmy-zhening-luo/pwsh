$Private:PROJECT_ROOT = "$PSScriptRoot\.."

[hashtable]$Private:Compiled = @{
  Path = "$PROJECT_ROOT\Cmdlet\bin\Release\netstandard2.0\Good.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = "$PROJECT_ROOT\Module\Good\"
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
