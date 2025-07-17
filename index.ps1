$code = "$HOME\code"
$PROFILE_REPO = "$code\pwsh"
$Env:PSModulePath += ";$PROFILE_REPO\modules"
$PROFILE_REPO_INDEX = "$PROFILE_REPO\index.ps1" # Must match dot-sourced script in $PROFILE; see `.\profile.ps1.example`.

$PSDefaultParameterValues = @{
  "Format-Table:Wrap" = $true
  "Invoke-Item:Path"  = "."
}

. $PSScriptRoot\consts\index.ps1

. $PSScriptRoot\scripts\object\index.ps1
. $PSScriptRoot\scripts\system\index.ps1
. $PSScriptRoot\scripts\shell\index.ps1
. $PSScriptRoot\scripts\apps\index.ps1
