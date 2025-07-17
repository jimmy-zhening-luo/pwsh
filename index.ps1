$code = "$HOME\code"
$PROFILE_REPO = "$code\pwsh"
$PROFILE_REPO_INDEX = "$PROFILE_REPO\index.ps1" # Must match dot-scope script in `$HOME\Documents\PowerShell\profile.ps1`
$PROFILE_REPO_MODULES = "$PROFILE_REPO\modules"

$Env:PSModulePath += ";$PROFILE_REPO_MODULES"
$PSDefaultParameterValues = @{
  "Format-Table:Wrap" = $true
  "Invoke-Item:Path"  = "."
}

. $PSScriptRoot\consts\index.ps1

. $PSScriptRoot\scripts\object\index.ps1
. $PSScriptRoot\scripts\system\index.ps1
. $PSScriptRoot\scripts\shell\index.ps1
. $PSScriptRoot\scripts\code\index.ps1
. $PSScriptRoot\scripts\apps\index.ps1
