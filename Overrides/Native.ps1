$ReadOnly = @{ Option = 'Readonly' }
$ReadOnlyAll = @{ Option = 'Readonly', 'AllScope' }

New-Alias clip Set-Clipboard @ReadOnly # default: clip.exe
New-Alias verb Get-VerbList @ReadOnly # default: implicit Get-Verb
New-Alias run Invoke-CommandPrompt @ReadOnlyAll # conflict: nvm/run.cmd
