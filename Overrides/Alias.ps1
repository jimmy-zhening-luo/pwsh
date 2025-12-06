$Readonly = @{
  Option = 'ReadOnly'
}
$ReadonlyConstant = @{
  Option = 'ReadOnly', 'Constant'
}
$ReadonlyAllScope = @{
  Option = 'ReadOnly', 'AllScope'
}

# Reassign PS alias
Set-Alias clear Shell\Clear-Line # was: Clear-Host
Set-Alias rd Shell\Remove-Directory # was: Remove-Item
Set-Alias man PSTool\Get-HelpOnline # was: Get-Help
Set-Alias gp Git\Get-Repository @ReadonlyConstant # was: Get-ItemProperty
Set-Alias gm Git\Write-Repository @ReadonlyConstant # was: Get-Member

# Mask implicit PS alias
New-Alias verb PSTool\Get-VerbList @Readonly # implicit Get-*

# Mask native PATH executable
New-Alias clip Set-Clipboard @Readonly # clip.exe
New-Alias run WindowsSystem\Invoke-CommandPrompt @ReadonlyAllScope # nvm/run.cmd
