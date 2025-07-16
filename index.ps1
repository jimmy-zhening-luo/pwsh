$code = "$HOME\code"
$PROFILE_REPO = "$code\pwsh"
$PROFILE_REPO_INDEX = "$PROFILE_REPO\index.ps1" # Must match dot-scope script in `$HOME\Documents\PowerShell\profile.ps1`

$PSDefaultParameterValues = @{
  "Format-Table:Wrap" = $true
  "Invoke-Item:Path"  = "."
}

. $PSScriptRoot\consts\index.ps1

. $PSScriptRoot\object.ps1
. $PSScriptRoot\system\index.ps1
. $PSScriptRoot\shell\index.ps1
. $PSScriptRoot\code\index.ps1
. $PSScriptRoot\apps\index.ps1
