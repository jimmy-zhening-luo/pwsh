$ReadOnly = @{
  Option = 'Readonly'
}
$ReadOnlyAllScope = @{
  Option = 'Readonly', 'AllScope'
}

# Reassign PS alias
Set-Alias clear Shell\Clear-Line # was: Clear-Host
Set-Alias rd Shell\Remove-Directory # was: Remove-Item
Set-Alias man PSTool\Get-HelpOnline # was: Get-Help

# Mask implicit PS alias
New-Alias verb PSTool\Get-VerbList @ReadOnly # implicit Get-*

# Mask native PATH executable
New-Alias clip Set-Clipboard @ReadOnly # clip.exe
New-Alias run WindowsSystem\Invoke-CommandPrompt @ReadOnlyAllScope # nvm/run.cmd
