$Env:PSModulePath += ";$PSScriptRoot\Module"

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Script\Parameter.psd1

. $PSScriptRoot\Script\Alias.ps1

[hashtable]$Private:Compiled = @{
  Path = "$HOME\code\pwsh\Cmdlet\Good\bin\Release\net10.0\Good.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = "$HOME\code\pwsh\Module\Good"
    Force       = $True
  }
  [hashtable]$Private:ExistingInstall = @{
    Path = Join-Path $Install.Destination Good.dll
  }

  if (
    -not (Test-Path @ExistingInstall) -or (
      Get-Item @ExistingInstall
    ).LastWriteTime -ne (
      Get-Item @Compiled
    ).LastWriteTime
  ) {
    Copy-Item @Compiled @Install
  }
}
