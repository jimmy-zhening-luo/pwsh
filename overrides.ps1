Set-Alias clear Clear-Line # default: Clear-Host
Set-Alias rd Remove-Folder # default: Remove-Item

$ReadOnly = @{
  Option = 'ReadOnly'
}

$ReadOnlyAll = @{
  Option = @(
    'ReadOnly'
    'AllScope'
  )
}

New-Alias clip Set-Clipboard @ReadOnly # default: clip.exe
New-Alias verb Get-VerbPowerShell @ReadOnly # default: implicit Get-Verb
New-Alias run Invoke-CommandPrompt @ReadOnlyAll # conflict: nvm/run.cmd
New-Alias wg winget @ReadOnly # conflict: WireGuard/wg.exe
