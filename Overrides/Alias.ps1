Set-Alias clear Clear-Line # default: Clear-Host
Set-Alias rd Remove-Directory # default: Remove-Item
Set-Alias man Get-HelpOnline # default: Get-Help

$Option = , 'Readonly'
$ReadOnly = @{ Option = $Option }
$ReadOnlyAll = @{ Option = $Option + 'AllScope' }

New-Alias clip Set-Clipboard @ReadOnly # default: clip.exe
New-Alias verb Get-VerbPowerShell @ReadOnly # default: implicit Get-Verb
New-Alias run Invoke-CommandPrompt @ReadOnlyAll # conflict: nvm/run.cmd
