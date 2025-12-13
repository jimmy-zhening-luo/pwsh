$Env:PSModulePath += ";$PSScriptRoot\Module"

$Global:PSDefaultParameterValues = Import-PowerShellDataFile -Path $PSScriptRoot\Overrides\Parameter.psd1

. $PSScriptRoot\Overrides\Alias.ps1

[hashtable]$Private:Compiled = @{
  Path = "$HOME\code\pwsh\Cmdlet\Good\bin\Release\net10.0\Good.dll"
}
if (Test-Path @Compiled) {
  [hashtable]$Private:Install = @{
    Destination = "$HOME\code\pwsh\Module\Good"
    Force       = $True
  }
  Copy-Item @Compiled @Install
}
