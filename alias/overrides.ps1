Set-Alias clear Clear-Line # default: Clear-Host
Set-Alias rd Remove-Folder # default: Remove-Item

New-Alias -Option ReadOnly clip Set-Clipboard # default: clip.exe
New-Alias -Option ReadOnly verb Get-VerbPowerShell # default: implicit Get-Verb
New-Alias -Option AllScope, ReadOnly run Invoke-CommandPrompt # conflict: nvm/run.cmd
New-Alias -Option ReadOnly wg winget # conflict: WireGuard/wg.exe
