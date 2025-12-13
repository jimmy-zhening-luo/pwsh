$Env:PSModulePath += ";$PSScriptRoot\Module"

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Script\Parameter.psd1

. $PSScriptRoot\Script\Alias.ps1

[hashtable]$Private:Compiled = @{
  Path = "$PSScriptRoot\Cmdlet\bin\Release\net10.0\Good.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = "$PSScriptRoot\Module\Good"
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
